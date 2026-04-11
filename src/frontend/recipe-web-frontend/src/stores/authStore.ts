import { defineStore } from 'pinia'
import type { User } from '../types/profile/user'
import {
    AuthApi,
    ProfileApi,
    Configuration,
    type LoginResponse,
    type Profile,
} from 'recipe-api-client'
import { toErrorMessage } from '../utils/identityErrors'

const config = new Configuration({ basePath: 'http://localhost:9393/api', credentials: 'include' })
const authApi = new AuthApi(config)
const profileApi = new ProfileApi(config)

function mapLoginToUser(response: LoginResponse): User {
    return {
        id: response.id,
        name: response.displayName,
        email: response.email,
        avatarUrl: undefined,
    }
}

function mapProfileToUser(response: Profile, avatarUrl?: string): User {
    return {
        id: response.id,
        name: response.displayName,
        email: response.email,
        registered: response.registeredAt.toISOString().slice(0, 10),
        avatarUrl,
    }
}

export const useAuthStore = defineStore('auth', {
    state: () => ({
        currentUser: null as User | null,
        authLoading: false,
        authError: null as string | null,
    }),

    actions: {
        login(user: User) {
            this.currentUser = user
            this.authError = null
        },
        async loginWithCredentials(email: string, password: string) {
            this.authLoading = true
            this.authError = null

            try {
                const response = await authApi.login({
                    loginRequest: { email, password },
                })

                this.currentUser = mapLoginToUser(response)
                return this.currentUser
            } catch (error) {
                this.currentUser = null
                this.authError = await toErrorMessage(error)
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
                return this.currentUser
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
        logout() {
            this.currentUser = null
            this.authError = null
        },
        updateUser(updates: Partial<User>) {
            if (this.currentUser) {
                this.currentUser = { ...this.currentUser, ...updates }
            }
        },
        async fetchOwnProfile() {
            this.authLoading = true
            this.authError = null

            try {
                const [profileResult, imageResult] = await Promise.allSettled([
                    profileApi.getOwnProfile(),
                    profileApi.getOwnProfileImage(),
                ])

                if (profileResult.status === 'fulfilled') {
                    const avatarUrl =
                        imageResult.status === 'fulfilled'
                            ? URL.createObjectURL(imageResult.value)
                            : this.currentUser?.avatarUrl
                    this.currentUser = mapProfileToUser(profileResult.value, avatarUrl)
                } else {
                    this.authError = await toErrorMessage(profileResult.reason)
                }
            } finally {
                this.authLoading = false
            }
        },
        async updateOwnProfile(params: {
            name?: string
            password?: string | null
            imageFile?: File | null
        }) {
            this.authLoading = true
            this.authError = null

            try {
                await profileApi.updateOwnProfile({
                    displayName: params.name ?? null,
                    password: params.password || null,
                    profileImage: params.imageFile ?? null,
                })
                await this.fetchOwnProfile()
            } catch (error) {
                this.authError = await toErrorMessage(error)
                throw error
            } finally {
                this.authLoading = false
            }
        },
        async deleteOwnProfile() {
            if (!this.currentUser?.id) {
                this.authError = 'Nem található bejelentkezett felhasználó.'
                return
            }

            this.authLoading = true
            this.authError = null

            try {
                await profileApi.deleteProfileById({ id: this.currentUser.id })
                this.logout()
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
        userName: (state) => state.currentUser?.name || 'Vendég',
        getUserId: (state) => state.currentUser?.id || '1',
    },
})
