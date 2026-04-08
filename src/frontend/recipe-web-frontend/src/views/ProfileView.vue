<script setup lang="ts">
import OwnRecipes from '@/components/recipe/OwnRecipes.vue'
import ProfileHeader from '@/components/profile/ProfileHeader.vue'
import { ref, watch, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import { useRecipePaginationStore } from '@/stores/recipePaginationStore'
import type { User } from '@/types/profile/user'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const recipePagination = useRecipePaginationStore()
const router = useRouter()

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
const deleteDialogOpen = ref(false)
const ownRecipesPageSize = 27

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
        await loadOwnRecipesPage(0)
    }
})

async function loadOwnRecipesPage(pageNumber: number) {
    if (!auth.currentUser) return

    await recipePagination.loadOwnRecipesPage(auth.currentUser.id, pageNumber, ownRecipesPageSize)
}

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

function openDeleteDialog() {
    if (editing.value || auth.authLoading) return
    deleteDialogOpen.value = true
}

function closeDeleteDialog() {
    deleteDialogOpen.value = false
}

async function confirmDeleteProfile() {
    try {
        await auth.deleteOwnProfile()
        deleteDialogOpen.value = false
        await router.push('/login')
    } catch {
        // auth.authError is already set
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
            @requestDelete="openDeleteDialog"
        />
        <p
            v-if="auth.authError"
            class="mt-4 text-sm rounded-lg border border-red-200 bg-red-50 text-red-700 px-3 py-2"
        >
            {{ auth.authError }}
        </p>

        <div
            v-if="deleteDialogOpen"
            class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
        >
            <div class="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
                <h2 class="text-lg font-semibold text-gray-900">Profil törlése</h2>
                <p class="mt-2 text-sm text-gray-600">
                    Biztosan törölni szeretnéd a profilodat? Ez a művelet nem visszavonható.
                </p>
                <div class="mt-5 flex justify-end gap-2">
                    <button
                        @click="closeDeleteDialog"
                        :disabled="auth.authLoading"
                        class="rounded border px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
                    >
                        Mégse
                    </button>
                    <button
                        @click="confirmDeleteProfile"
                        :disabled="auth.authLoading"
                        class="rounded bg-red-600 px-4 py-2 text-white hover:bg-red-700 disabled:opacity-50"
                    >
                        {{ auth.authLoading ? 'Törlés...' : 'Igen, törlöm' }}
                    </button>
                </div>
            </div>
        </div>

        <OwnRecipes
            :recipes="recipePagination.ownRecipes"
            :pagination="recipePagination.ownRecipesPagination"
            :loading="recipePagination.ownRecipesLoading"
            @pageChange="loadOwnRecipesPage"
            class="mt-8"
        />
    </main>
</template>
