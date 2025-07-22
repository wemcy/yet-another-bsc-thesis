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
            <input
                type="text"
                placeholder="Keresés..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </div>

        <ul class="flex justify-end space-x-6 text-gray-700">
            <li><router-link to="/recipes" class="hover:text-blue-600">Receptek</router-link></li>
            <li>
                <router-link to="/new-recipe" class="hover:text-blue-600">Új recept</router-link>
            </li>
        </ul>

        <!-- Jobb oldalon csak avatar -->
        <div class="flex items-center">
            <template v-if="auth.currentUser">
                <router-link to="/profile" class="ml-4">
                    <ProfileAvatar :src="auth.currentUser?.avatarUrl" size="md" />
                </router-link>
            </template>
            <template v-else>
                <button
                    @click="dummyLogin"
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
import { useKeycloak } from '@josempgon/vue-keycloak'

const auth = useAuthStore()
const { keycloak } = useKeycloak()
// const dummyUser: User = {
//     id: 'dummy-user',
//     name: 'Teszt Elek',
//     email: 'teszt@valami.hu',
//     password: 'titok',
//     registered: '2023-11-01',
// }

function dummyLogin() {
    keycloak.value
        ?.login({
            redirectUri: window.location.origin,
        })
        .catch((error) => {
            console.error('Keycloak login failed:', error)
        })
}
</script>
