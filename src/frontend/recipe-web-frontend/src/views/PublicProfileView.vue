<script setup lang="ts">
import PublicProfileHeader from '@/components/profile/PublicProfileHeader.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import { ProfileApi, Configuration, type ProfileSummary, ResponseError } from 'recipe-api-client'
import { MapApiRecipeToRecipe } from '@/types/recipe/recipe.mappers'
import { parsePaginationState } from '@/utils/pagination'
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import { getSingleRouteParam } from '@/utils/routeParams'
import type { PaginationState, Recipe } from '@/types/recipe/recipe'
import { computed, ref, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const config = new Configuration({ basePath: '/api', credentials: 'include' })
const profileApi = new ProfileApi(config)

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
const userId = ref(getSingleRouteParam(route.params, 'id') ?? '')

const profile = ref<ProfileSummary | null>(null)
const avatarUrl = ref<string | undefined>(undefined)
const profileLoading = ref(true)
const profileError = ref<string | null>(null)
const requiresLogin = ref(false)
const adminActionMessage = ref<string | null>(null)
const deleteDialogOpen = ref(false)
const makeAdminDialogOpen = ref(false)

const recipes = ref<Recipe[]>([])
const recipesLoading = ref(false)
const recipesPagination = ref<PaginationState>({
    pageNumber: 0,
    pageSize: 27,
    totalCount: 0,
    pageCount: 0,
    hasNextPage: false,
    hasPreviousPage: false,
})

const canManageProfile = computed(
    () => auth.isAdmin && !!profile.value && auth.currentUser?.id !== userId.value,
)

async function loadProfile() {
    profileLoading.value = true
    profileError.value = null
    requiresLogin.value = false
    try {
        const [profileResult, imageResult] = await Promise.allSettled([
            profileApi.getProfileById({ id: userId.value }),
            profileApi.getProfileImageById({ id: userId.value }),
        ])

        if (profileResult.status === 'fulfilled') {
            profile.value = profileResult.value
            avatarUrl.value =
                imageResult.status === 'fulfilled'
                    ? URL.createObjectURL(imageResult.value)
                    : undefined
        } else {
            const reason = profileResult.reason
            const status = reason instanceof ResponseError ? reason.response.status : undefined
            if (status === 401 || status === 403) {
                requiresLogin.value = true
                profileError.value =
                    'A felhasználó profiljához csak bejelentkezés után férhetsz hozzá!'
            } else if (status === 404) {
                profileError.value = 'A profil nem található.'
            } else {
                profileError.value = 'Hiba történt a profil betöltésekor.'
            }
        }
    } catch {
        profileError.value = 'Hiba történt a profil betöltésekor.'
    } finally {
        profileLoading.value = false
    }
}

function getRecipesPageFromQuery() {
    const value = route.query.recipesPage
    const raw = typeof value === 'string' ? Number.parseInt(value, 10) : NaN
    if (Number.isNaN(raw) || raw < 1) return 0
    return raw - 1
}

async function loadRecipesPage(pageNumber: number) {
    recipesLoading.value = true
    try {
        const response = await api.getRecipesByAuthorIdRaw({
            id: userId.value,
            page: pageNumber,
            pageSize: recipesPagination.value.pageSize,
        })
        const body = await response.value()
        recipes.value = body.map((r) => MapApiRecipeToRecipe(r))
        recipesPagination.value = parsePaginationState(
            response.raw.headers.get('X-Pagination'),
            pageNumber,
            recipesPagination.value.pageSize,
        )
    } finally {
        recipesLoading.value = false
    }
}

async function requestRecipesPage(pageNumber: number) {
    await router.push({
        query: { ...route.query, recipesPage: String(pageNumber + 1) },
    })
}

function openDeleteDialog() {
    if (!canManageProfile.value || auth.authLoading) return
    adminActionMessage.value = null
    deleteDialogOpen.value = true
}

function closeDeleteDialog() {
    deleteDialogOpen.value = false
}

function openMakeAdminDialog() {
    if (!canManageProfile.value || auth.authLoading) return
    adminActionMessage.value = null
    makeAdminDialogOpen.value = true
}

function closeMakeAdminDialog() {
    makeAdminDialogOpen.value = false
}

async function confirmDeleteProfile() {
    try {
        await auth.deleteProfileById(userId.value)
        deleteDialogOpen.value = false
        await router.push({ name: 'Home' })
    } catch {
        // auth.authError is already set
    }
}

async function confirmMakeAdmin() {
    try {
        await auth.makeUserAdminById(userId.value)
        makeAdminDialogOpen.value = false
        adminActionMessage.value = 'A felhasználó admin jogosultságot kapott.'
    } catch {
        // auth.authError is already set
    }
}

watch(
    () => route.query.recipesPage,
    async () => {
        await loadRecipesPage(getRecipesPageFromQuery())
    },
)

watch(
    () => route.params.id,
    async (newId) => {
        if (!newId) return
        userId.value = Array.isArray(newId) ? (newId[0] ?? '') : newId
        if (!userId.value) return

        if (auth.currentUser && auth.currentUser.id === userId.value) {
            router.replace({ name: 'Profile' })
            return
        }

        await loadProfile()
        await loadRecipesPage(getRecipesPageFromQuery())
    },
)

onMounted(async () => {
    if (auth.currentUser && auth.currentUser.id === userId.value) {
        router.replace({ name: 'Profile' })
        return
    }

    await loadProfile()
    await loadRecipesPage(getRecipesPageFromQuery())
})
</script>

<template>
    <main class="max-w-5xl mx-auto py-8 px-4">
        <div v-if="profileLoading" class="text-center py-20 text-gray-400">Betöltés...</div>

        <div
            v-else-if="profileError"
            class="max-w-xl mx-auto mt-10 rounded-xl border border-dashed border-gray-300 bg-white p-8 text-center shadow-sm"
        >
            <p class="text-gray-600">{{ profileError }}</p>
            <button
                v-if="requiresLogin"
                @click="router.push({ name: 'Login', query: { redirect: route.fullPath } })"
                class="mt-4 bg-blue-600 text-white px-5 py-2.5 rounded-lg hover:bg-blue-700 transition"
            >
                Bejelentkezés
            </button>
        </div>

        <template v-else-if="profile">
            <PublicProfileHeader :displayName="profile.displayName" :avatarUrl="avatarUrl" />
            <div v-if="canManageProfile" class="mt-6 flex flex-wrap gap-3">
                <button
                    type="button"
                    @click="openDeleteDialog"
                    :disabled="auth.authLoading"
                    class="rounded-lg bg-red-600 px-5 py-2 text-white transition hover:bg-red-700 disabled:opacity-50"
                >
                    Profil törlése
                </button>
                <button
                    type="button"
                    @click="openMakeAdminDialog"
                    :disabled="auth.authLoading"
                    class="rounded-lg border border-blue-300 bg-white px-5 py-2 text-blue-700 transition hover:bg-blue-50 disabled:opacity-50"
                >
                    Adminná tétel
                </button>
            </div>
            <p
                v-if="auth.authError"
                class="mt-4 text-sm rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-red-700"
            >
                {{ auth.authError }}
            </p>
            <p
                v-else-if="adminActionMessage"
                class="mt-4 text-sm rounded-lg border border-emerald-200 bg-emerald-50 px-3 py-2 text-emerald-700"
            >
                {{ adminActionMessage }}
            </p>

            <div class="mt-10">
                <h3 class="text-xl font-bold mb-4">Receptjei</h3>
                <div
                    v-if="recipesLoading"
                    class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6"
                >
                    <RecipeCardSkeleton v-for="n in recipesPagination.pageSize" :key="n" />
                </div>
                <div
                    v-else-if="recipes.length === 0"
                    class="rounded-lg border border-dashed p-6 text-sm"
                >
                    Ennek a felhasználónak még nincsenek receptjei.
                </div>
                <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    <RecipeCard v-for="recipe in recipes" :key="recipe.id" :recipe="recipe" />
                </div>
                <RecipePagination
                    :currentPage="recipesPagination.pageNumber"
                    :totalPages="recipesPagination.pageCount"
                    :hasNextPage="recipesPagination.hasNextPage"
                    :hasPreviousPage="recipesPagination.hasPreviousPage"
                    :disabled="recipesLoading"
                    @pageChange="requestRecipesPage"
                />
            </div>

            <div
                v-if="deleteDialogOpen"
                class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
            >
                <div class="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
                    <h2 class="text-lg font-semibold text-gray-900">Profil törlése</h2>
                    <p class="mt-2 text-sm text-gray-600">
                        Biztosan törölni szeretnéd ezt a profilt? Ez a művelet nem visszavonható.
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

            <div
                v-if="makeAdminDialogOpen"
                class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
            >
                <div class="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
                    <h2 class="text-lg font-semibold text-gray-900">Adminná tétel</h2>
                    <p class="mt-2 text-sm text-gray-600">
                        Biztosan admin jogosultságot szeretnél adni ennek a felhasználónak?
                    </p>
                    <div class="mt-5 flex justify-end gap-2">
                        <button
                            @click="closeMakeAdminDialog"
                            :disabled="auth.authLoading"
                            class="rounded border px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
                        >
                            Mégse
                        </button>
                        <button
                            @click="confirmMakeAdmin"
                            :disabled="auth.authLoading"
                            class="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 disabled:opacity-50"
                        >
                            {{ auth.authLoading ? 'Mentés...' : 'Igen, admin legyen' }}
                        </button>
                    </div>
                </div>
            </div>
        </template>
    </main>
</template>
