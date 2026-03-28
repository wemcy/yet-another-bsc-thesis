<script setup lang="ts">
import OwnRecipes from '@/components/recipe/OwnRecipes.vue'
import ProfileHeader from '@/components/profile/ProfileHeader.vue'
import { ref, watch, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import { useRecipeStore } from '@/stores/recipeStore'
import type { User } from '@/types/profile/user'

const auth = useAuthStore()
const recipes = useRecipeStore()

const editing = ref(false)
const profile = ref<User>(
    auth.currentUser ?? {
        id: '',
        name: '',
        email: '',
        avatarUrl: undefined,
        registered: '',
    },
)
const imageFile = ref<File | null>(null)
const imageUrl = ref<string | undefined>(profile.value.avatarUrl)
const passwordConfirm = ref('')

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

onMounted(async () => {
    await auth.fetchOwnProfile()
    if (auth.currentUser) {
        recipes.fetchOwnRecipes(auth.currentUser.id)
    }
})

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
        imageUrl.value = URL.createObjectURL(file)
    }
}

async function saveEdit() {
    if (!validateProfile()) return

    try {
        await auth.updateOwnProfile({
            name: profile.value.name,
            password: profile.value.password || null,
            imageFile: imageFile.value,
        })
        editing.value = false
        passwordConfirm.value = ''
        imageFile.value = null
    } catch {
        // profileStore.profileError is already set
    }
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
        <p
            v-if="auth.authError"
            class="mt-4 text-sm rounded-lg border border-red-200 bg-red-50 text-red-700 px-3 py-2"
        >
            {{ auth.authError }}
        </p>
        <OwnRecipes :recipes="recipes.ownRecipes" class="mt-8" />
    </main>
</template>
