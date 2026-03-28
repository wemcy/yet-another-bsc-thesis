import { defineStore } from 'pinia'
import type { User } from '../types/profile/user'
import { AuthApi, Configuration, type LoginResponse } from 'recipe-api-client'
import { toErrorMessage } from '../utils/identityErrors'

const authApi = new AuthApi(
    new Configuration({
        // Keep auth calls on the same API gateway host/port used by the rest of the app.
        basePath: 'http://localhost:9393/api',
    }),
)

function mapResponseToUser(response: LoginResponse): User {
    return {
        id: response.id,
        name: response.displayName,
        email: response.email,
        avatarUrl: undefined,
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

                this.currentUser = mapResponseToUser(response)
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

                this.currentUser = mapResponseToUser(response)
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
    },

    getters: {
        isLoggedIn: (state) => !!state.currentUser,
        userName: (state) => state.currentUser?.name || 'Vendég',
        getUserId: (state) => state.currentUser?.id || '1',
    },
})
