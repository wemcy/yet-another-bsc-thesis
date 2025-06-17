import { defineStore } from 'pinia'
import type { User } from '../types/profile/user'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        currentUser: null as User | null,
    }),

    actions: {
        login(user: User) {
            this.currentUser = user
        },
        logout() {
            this.currentUser = null
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
    },
})
