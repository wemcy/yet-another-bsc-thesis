import { defineStore } from 'pinia'
import type { User } from '../types/profile/user'
import {
    AuthApi,
    ProfileApi,
    Configuration,
    type LoginResponse,
    type Profile,
    UserRole,
    type UpdateOwnProfileRequest,
} from 'recipe-api-client'
import { toErrorMessage } from '../utils/identityErrors'

const config = new Configuration({ basePath: '/api', credentials: 'include' })
const authApi = new AuthApi(config)
const profileApi = new ProfileApi(config)

interface AuthState {
    currentUser: User | null
    authLoading: boolean
    authError: string | null
    sessionRestored: boolean
}

const ROLE_STORAGE_KEY = 'recipe-app-user-roles'

function readStoredRoles(): User['roles'] {
    if (typeof window === 'undefined') return []

    const raw = window.localStorage.getItem(ROLE_STORAGE_KEY)
    if (!raw) return []

    try {
        const parsed = JSON.parse(raw)
        return Array.isArray(parsed) ? (parsed as User['roles']) : []
    } catch {
        return []
    }
}

function storeRoles(roles: User['roles']) {
    if (typeof window === 'undefined') return

    window.localStorage.setItem(ROLE_STORAGE_KEY, JSON.stringify(roles))
}

function clearStoredRoles() {
    if (typeof window === 'undefined') return

    window.localStorage.removeItem(ROLE_STORAGE_KEY)
}

function mapLoginToUser(response: LoginResponse): User {
    return {
        id: response.id,
        name: response.displayName,
        email: response.email,
        roles: response.roles,
        avatarUrl: undefined,
    }
}

function mapProfileToUser(response: Profile, existingRoles?: User['roles']): User {
    return {
        id: response.id,
        name: response.displayName,
        email: response.email,
        roles: existingRoles ?? [],
        registered: response.registeredAt.toISOString().slice(0, 10),
        avatarUrl: '/api/profile/me/image',
    }
}

export const useAuthStore = defineStore('auth', {
    state: (): AuthState => ({
        currentUser: null,
        authLoading: false,
        authError: null,
        sessionRestored: false,
    }),

    actions: {
        login(user: User) {
            this.currentUser = user
            this.authError = null
            storeRoles(user.roles)
        },
        async loginWithCredentials(email: string, password: string) {
            this.authLoading = true
            this.authError = null

            try {
                const response = await authApi.login({
                    loginRequest: { email, password },
                })

                this.currentUser = mapLoginToUser(response)
                storeRoles(this.currentUser.roles)
                return this.currentUser
            } catch (error) {
                this.currentUser = null
                this.authError = await toErrorMessage(error)
                clearStoredRoles()
                throw error
            } finally {
                this.authLoading = false
            }
        },
        async registerWithCredentials(name: string, email: string, password: string) {
            this.authLoading = true
            this.authError = null

            try {
                const response = await authApi.register({
                    registerRequest: { email, password, displayName: name },
                })

                this.currentUser = mapLoginToUser(response)
                storeRoles(this.currentUser.roles)
                return this.currentUser
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
        async logout() {
            this.currentUser = null
            this.authError = null
            clearStoredRoles()
            try {
                await authApi.logout()
            } catch {
                // Even if logout API call fails, we still want to clear the local session
            }
        },
        updateUser(updates: Partial<User>) {
            if (this.currentUser) {
                this.currentUser = { ...this.currentUser, ...updates }
                storeRoles(this.currentUser.roles)
            }
        },
        async ensureSession() {
            await this.tryRestoreSession()
        },
        async tryRestoreSession() {
            if (this.sessionRestored) {
                return
            }

            this.sessionRestored = true

            await this.fetchOwnProfile()
            this.authError = null
        },
        async fetchOwnProfile() {
            this.authLoading = true
            this.authError = null

            try {
                const profile = await profileApi.getOwnProfile()
                this.currentUser = mapProfileToUser(profile, this.currentUser?.roles ?? readStoredRoles())
                storeRoles(this.currentUser.roles)
            } catch (reason) {
                this.authError = await toErrorMessage(reason)
            } finally {
                this.authLoading = false
            }
        },
        async updateOwnProfile(params: UpdateOwnProfileRequest) {
            this.authLoading = true
            try {
                this.authError = null
                await profileApi.updateOwnProfile(params)
                await this.fetchOwnProfile()
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
        async deleteOwnProfile() {
            if (!this.isLoggedIn) {
                this.authError = 'Nem található bejelentkezett felhasználó.'
                return
            }

            this.authLoading = true
            this.authError = null

            await this.deleteProfileById(this.getUserId!)
            await this.logout()
        },
        async deleteProfileById(id: string) {
            this.authLoading = true
            this.authError = null

            try {
                await profileApi.deleteProfileById({ id })
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
        async makeUserAdminById(id: string) {
            this.authLoading = true
            this.authError = null

            try {
                await profileApi.addUserRoleById({
                    id,
                    addUserRoleByIdRequest: { role: UserRole.Admin },
                })
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
    },

    getters: {
        isLoggedIn: (state) => !!state.currentUser,
        isAdmin: (state) => state.currentUser?.roles?.includes('Admin') ?? false,
        userName: (state) => state.currentUser?.name,
        getUserId: (state) => state.currentUser?.id,
    },
})
