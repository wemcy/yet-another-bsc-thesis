<script setup lang="ts">
import OwnRecipes from '@/components/recipe/OwnRecipes.vue'
import ProfileHeader from '@/components/profile/ProfileHeader.vue'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import { useRecipeStore } from '@/stores/recipeStore'
import type { User } from '@/types/profile/user'

const auth = useAuthStore()
const recipes = useRecipeStore()

const editing = ref(false)
const profile = ref<User>(
    auth.currentUser ?? {
        id: '', // vagy egyéb safe default
        name: '',
        email: '',
        avatarUrl: undefined,
        registered: '',
        // stb.
    },
)
const imageFile = ref<File | null>(null)
const imageUrl = ref<string | undefined>(profile.value.avatarUrl)
const passwordConfirm = ref('')
import { watch } from 'vue'

watch(
    () => auth.currentUser,
    (newUser) => {
        if (newUser) {
            profile.value = { ...newUser }
            imageUrl.value = newUser.avatarUrl ?? undefined
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
    if (!profile.value.name?.trim()) errors.value.name = 'A név kötelező.'
    if (!profile.value.email?.trim() || !profile.value.email.includes('@'))
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
    profile.value.password = ''
    passwordConfirm.value = ''
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
        ...(profile.value.password ? { password: profile.value.password } : {}),
    })
    editing.value = false
    passwordConfirm.value = ''
}
</script>

<template>
    <main class="max-w-5xl mx-auto py-8 px-4">
        <ProfileHeader
            :profile="profile"
            :editing="editing"
            :errors="errors"
            :passwordConfirm="passwordConfirm"
            :imageUrl="imageUrl"
            @edit="startEdit"
            @cancel="cancelEdit"
            @save="saveEdit"
            @imageChange="handleImageChange"
            @updateProfile="(v) => (profile = v)"
            @updatePasswordConfirm="(v) => (passwordConfirm = v)"
        />
        <OwnRecipes :recipes="recipes.recipes" class="mt-8" />
    </main>
</template>
