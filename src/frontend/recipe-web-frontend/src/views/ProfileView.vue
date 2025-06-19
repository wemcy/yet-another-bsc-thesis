<script setup lang="ts">
import OwnRecipes from '@/components/recipe/OwnRecipes.vue'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import { useRecipeStore } from '@/stores/recipeStore'

const auth = useAuthStore()
const recipes = useRecipeStore()

const editing = ref(false)
const profile = ref({ ...auth.currentUser })
const imageFile = ref<File | null>(null)
const imageUrl = ref(profile.value.avatarUrl ?? null)
const passwordConfirm = ref('')
import { watch } from 'vue'

watch(
    () => auth.currentUser,
    (newUser) => {
        if (newUser) {
            profile.value = { ...newUser }
            imageUrl.value = newUser.avatarUrl ?? null
        }
    },
    { immediate: true },
)
const original = ref({ ...profile.value })
const errors = ref<{ name?: string; email?: string; password?: string; passwordConfirm?: string }>(
    {},
)

function validateProfile() {
    errors.value = {}
    if (!profile.value.name.trim()) errors.value.name = 'A név kötelező.'
    if (!profile.value.email.trim() || !profile.value.email.includes('@'))
        errors.value.email = 'Érvényes email kötelező.'
    if (editing.value && profile.value.password) {
        if (profile.value.password.length < 6)
            errors.value.password = 'A jelszónak legalább 6 karakteresnek kell lennie.'
        if (profile.value.password !== passwordConfirm.value)
            errors.value.passwordConfirm = 'A jelszavak nem egyeznek.'
    }
    return Object.keys(errors.value).length === 0
}

function startEdit() {
    original.value = { ...profile.value }
    editing.value = true
}

function cancelEdit() {
    profile.value = { ...original.value }
    editing.value = false
    errors.value = {}
    passwordConfirm.value = ''
}
function handleImageChange(e: Event) {
    const file = (e.target as HTMLInputElement)?.files?.[0]
    if (file) {
        imageFile.value = file
        // helyileg csak a böngészőben, base64 vagy object url
        imageUrl.value = URL.createObjectURL(file)
        // mentésnél majd ezt tároljuk az authStore-ban
    }
}
function saveEdit() {
    if (!validateProfile()) return

    auth.updateUser({
        name: profile.value.name,
        email: profile.value.email,
        avatarUrl: imageUrl.value,
        // password: profile.value.password, // csak ha meg van adva!
    })
    editing.value = false
    passwordConfirm.value = ''
}
</script>

<template>
    <div class="max-w-xl mx-auto bg-white shadow p-6 rounded-md mt-10">
        <div v-if="auth.currentUser" class="space-y-4">
            <div class="max-w-xl mx-auto bg-white shadow p-6 rounded-md mt-10">
                <div class="flex justify-between items-start mb-6">
                    <div>
                        <label class="block font-semibold mb-1">Profil kép</label>
                        <div
                            class="w-16 h-16 border rounded-full flex items-center justify-center text-gray-400 overflow-hidden"
                        >
                            <img
                                v-if="imageUrl || profile.avatarUrl"
                                :src="imageUrl || profile.avatarUrl"
                                alt="Profilkép"
                                class="object-cover w-16 h-16"
                            />
                            <span v-else class="text-2xl">👤</span>
                        </div>
                        <div v-if="editing" class="mt-2">
                            <input type="file" accept="image/*" @change="handleImageChange" />
                        </div>
                    </div>
                    <button
                        v-if="!editing"
                        @click="startEdit"
                        class="border px-4 py-1 rounded hover:bg-gray-100"
                    >
                        Szerkesztés
                    </button>
                </div>
                <div>
                    <label class="block font-semibold">Név</label>
                    <input
                        v-if="editing"
                        v-model="profile.name"
                        class="border rounded px-3 py-1 w-full"
                    />
                    <p v-else>{{ auth.currentUser.name }}</p>
                    <p v-if="errors.name" class="text-red-600 text-sm">{{ errors.name }}</p>
                </div>

                <div>
                    <label class="block font-semibold">Email</label>
                    <input
                        v-if="editing"
                        v-model="profile.email"
                        class="border rounded px-3 py-1 w-full"
                    />
                    <p v-else>{{ auth.currentUser.email }}</p>
                    <p v-if="errors.email" class="text-red-600 text-sm">{{ errors.email }}</p>
                </div>

                <div>
                    <label class="block font-semibold">Jelszó</label>
                    <input
                        v-if="editing"
                        v-model="profile.password"
                        type="password"
                        class="border rounded px-3 py-1 w-full"
                    />
                    <p v-if="errors.password" class="text-red-600 text-sm">{{ errors.password }}</p>
                    <label v-if="editing" class="block font-semibold">Jelszó megerősítése</label>
                    <input
                        v-if="editing"
                        v-model="passwordConfirm"
                        type="password"
                        class="border rounded px-3 py-1 w-full"
                    />
                    <p v-if="errors.passwordConfirm" class="text-red-600 text-sm">
                        {{ errors.passwordConfirm }}
                    </p>
                    <p v-else-if="!editing">********</p>
                </div>

                <div>
                    <label class="block font-semibold">Regisztráció dátuma</label>
                    <p>{{ auth.currentUser.registered }}</p>
                </div>
            </div>

            <div v-if="editing" class="flex justify-end gap-2 mt-6">
                <button @click="cancelEdit" class="border px-4 py-1 rounded hover:bg-gray-100">
                    Mégse
                </button>
                <button
                    @click="saveEdit"
                    class="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
                >
                    Mentés
                </button>
            </div>
            <div class="mt-10">
                <OwnRecipes :recipes="recipes.recipes" />
            </div>
        </div>

        <div v-else>
            <p class="text-red-500">Nem vagy bejelentkezve.</p>
        </div>
    </div>
</template>
