<template>
    <nav
        class="bg-white shadow-md px-6 py-4 flex flex-col md:flex-row items-center md:justify-between gap-4 md:gap-0"
    >
        <!-- Logó / Főoldal link -->
        <router-link to="/" class="text-2xl font-bold text-gray-800 hover:text-blue-600 transition">
            ReceptApp
        </router-link>

        <!-- Kereső -->
        <div class="w-full md:w-1/2">
            <RecipeSearchAutocomplete />
        </div>

        <ul class="flex justify-end space-x-6 text-gray-700">
            <li><router-link to="/recipes" class="hover:text-blue-600">Receptek</router-link></li>
            <li>
                <router-link to="/new-recipe" class="hover:text-blue-600">Új recept</router-link>
            </li>
        </ul>

        <!-- Jobb oldalon profil és auth akciók -->
        <div class="flex items-center gap-3">
            <template v-if="auth.currentUser">
                <router-link to="/profile" class="flex items-center gap-2 text-sm text-gray-700">
                    <ProfileAvatar :src="auth.currentUser?.avatarUrl" size="md" />
                    <span class="font-medium">{{ auth.currentUser.name }}</span>
                </router-link>
                <button
                    @click="logout"
                    class="border border-gray-300 text-gray-700 px-3 py-2 rounded-lg hover:bg-gray-100 transition"
                >
                    Kilépés
                </button>
            </template>
            <template v-else>
                <button
                    @click="login"
                    class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition"
                >
                    Belépés
                </button>
            </template>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'
import ProfileAvatar from '@/components/profile/ProfileAvatar.vue'
import RecipeSearchAutocomplete from '@/components/search/RecipeSearchAutocomplete.vue'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const router = useRouter()

function login() {
    router.push('/login')
}

function logout() {
    auth.logout()
    router.push('/login')
}
</script>
