<script setup lang="ts">
import PublicProfileHeader from '@/components/profile/PublicProfileHeader.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import { ProfileApi, Configuration, type ProfileSummary, ResponseError } from 'recipe-api-client'
import { MapApiRecipeToRecipe } from '@/types/recipe/recipe.mappers'
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import type { PaginationState, Recipe } from '@/types/recipe/recipe'
import { ref, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const config = new Configuration({ basePath: '/api', credentials: 'include' })
const profileApi = new ProfileApi(config)

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
const userId = ref(route.params.id as string)

const profile = ref<ProfileSummary | null>(null)
const avatarUrl = ref<string | undefined>(undefined)
const profileLoading = ref(true)
const profileError = ref<string | null>(null)
const requiresLogin = ref(false)

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

const parseBoolean = (value: unknown): boolean | undefined => {
    if (typeof value === 'boolean') return value
    return undefined
}

const parseNumber = (value: unknown): number | undefined => {
    if (typeof value === 'number' && Number.isFinite(value)) return value
    return undefined
}

const parsePaginationState = (
    headerValue: string | null,
    fallbackPageNumber: number,
    fallbackPageSize: number,
): PaginationState => {
    if (!headerValue) {
        return {
            pageNumber: fallbackPageNumber,
            pageSize: fallbackPageSize,
            totalCount: 0,
            pageCount: 0,
            hasNextPage: false,
            hasPreviousPage: fallbackPageNumber > 0,
        }
    }
    try {
        const raw = JSON.parse(headerValue) as Record<string, unknown>
        return {
            pageNumber:
                parseNumber(raw.pageNumber) ?? parseNumber(raw.PageNumber) ?? fallbackPageNumber,
            pageSize: parseNumber(raw.pageSize) ?? parseNumber(raw.PageSize) ?? fallbackPageSize,
            totalCount: parseNumber(raw.totalCount) ?? parseNumber(raw.TotalCount) ?? 0,
            pageCount: parseNumber(raw.pageCount) ?? parseNumber(raw.PageCount) ?? 0,
            hasNextPage: parseBoolean(raw.hasNextPage) ?? parseBoolean(raw.HasNextPage) ?? false,
            hasPreviousPage:
                parseBoolean(raw.hasPreviousPage) ??
                parseBoolean(raw.HasPreviousPage) ??
                fallbackPageNumber > 0,
        }
    } catch {
        return {
            pageNumber: fallbackPageNumber,
            pageSize: fallbackPageSize,
            totalCount: 0,
            pageCount: 0,
            hasNextPage: false,
            hasPreviousPage: fallbackPageNumber > 0,
        }
    }
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
        userId.value = newId as string

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
        </template>
    </main>
</template>
