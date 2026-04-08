<script setup lang="ts">
import { useRecipePaginationStore } from '@/stores/recipePaginationStore'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import { allergenList } from '@/types/recipe/allergens'
import { computed, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const paginationStore = useRecipePaginationStore()
const pageSize = 27
const skeletonCards = computed(() => Array.from({ length: pageSize }, (_, i) => i))
const route = useRoute()
const router = useRouter()
const includeAllergens = ref<string[]>([])
const excludeAllergens = ref<string[]>([])

const validAllergens = new Set<string>(allergenList)

const filteredRecipes = computed(() =>
    paginationStore.allRecipes.filter((recipe) => {
        const recipeAllergens = recipe.allergens as unknown as string[]
        const includesAll = includeAllergens.value.every((allergen) =>
            recipeAllergens.includes(allergen),
        )
        const includesAnyExcluded = excludeAllergens.value.some((allergen) =>
            recipeAllergens.includes(allergen),
        )
        return includesAll && !includesAnyExcluded
    }),
)

function parseAllergenQueryParam(param: unknown): string[] {
    if (typeof param !== 'string' || param.trim() === '') return []

    return [...new Set(param.split(',').map((value) => value.trim()))].filter((value) =>
        validAllergens.has(value),
    )
}

function getPageFromQuery() {
    const value = route.query.page
    const raw = typeof value === 'string' ? Number.parseInt(value, 10) : NaN
    if (Number.isNaN(raw) || raw < 1) return 0
    return raw - 1
}

function syncFiltersFromQuery() {
    includeAllergens.value = parseAllergenQueryParam(route.query.include)
    excludeAllergens.value = parseAllergenQueryParam(route.query.exclude).filter(
        (item) => !includeAllergens.value.includes(item),
    )
}

async function loadPage(pageNumber: number) {
    await paginationStore.loadAllRecipesPage(pageNumber, pageSize)
}

async function requestPage(pageNumber: number) {
    await router.push({
        query: {
            ...route.query,
            page: String(pageNumber + 1),
        },
    })
}

async function updateAllergenQuery() {
    await router.replace({
        query: {
            ...route.query,
            include:
                includeAllergens.value.length > 0 ? includeAllergens.value.join(',') : undefined,
            exclude:
                excludeAllergens.value.length > 0 ? excludeAllergens.value.join(',') : undefined,
        },
    })
}

async function toggleInclude(allergen: string) {
    if (includeAllergens.value.includes(allergen)) {
        includeAllergens.value = includeAllergens.value.filter((item) => item !== allergen)
    } else {
        includeAllergens.value = [...includeAllergens.value, allergen]
        excludeAllergens.value = excludeAllergens.value.filter((item) => item !== allergen)
    }

    await updateAllergenQuery()
}

async function toggleExclude(allergen: string) {
    if (excludeAllergens.value.includes(allergen)) {
        excludeAllergens.value = excludeAllergens.value.filter((item) => item !== allergen)
    } else {
        excludeAllergens.value = [...excludeAllergens.value, allergen]
        includeAllergens.value = includeAllergens.value.filter((item) => item !== allergen)
    }

    await updateAllergenQuery()
}

async function clearAllergenFilters() {
    includeAllergens.value = []
    excludeAllergens.value = []
    await updateAllergenQuery()
}

watch(
    () => [route.query.include, route.query.exclude],
    () => {
        syncFiltersFromQuery()
    },
    { immediate: true },
)

watch(
    () => route.query.page,
    async () => {
        await loadPage(getPageFromQuery())
    },
    { immediate: true },
)
</script>

<template>
    <main class="max-w-6xl mx-auto p-6">
        <h1 class="text-2xl font-bold mb-6">Receptek</h1>

        <section class="mb-6 rounded-xl border border-gray-200 bg-white p-4 shadow-sm">
            <div class="flex items-center justify-between mb-3">
                <h2 class="text-lg font-semibold text-gray-900">Allergén szűrő</h2>
                <button
                    type="button"
                    class="text-sm text-blue-600 hover:underline disabled:text-gray-400 disabled:no-underline"
                    :disabled="includeAllergens.length === 0 && excludeAllergens.length === 0"
                    @click="clearAllergenFilters"
                >
                    Szűrők törlése
                </button>
            </div>

            <div class="grid gap-6 md:grid-cols-2">
                <div>
                    <h3 class="text-sm font-medium text-gray-700 mb-2">Kötelezően tartalmazza</h3>
                    <div class="grid grid-cols-1 sm:grid-cols-2 gap-2">
                        <label
                            v-for="allergen in allergenList"
                            :key="`include-${allergen}`"
                            class="flex items-center gap-2 text-sm"
                        >
                            <input
                                type="checkbox"
                                :checked="includeAllergens.includes(allergen)"
                                @change="toggleInclude(allergen)"
                                class="accent-blue-600"
                            />
                            <span>{{ allergen }}</span>
                        </label>
                    </div>
                </div>

                <div>
                    <h3 class="text-sm font-medium text-gray-700 mb-2">Nem tartalmazhatja</h3>
                    <div class="grid grid-cols-1 sm:grid-cols-2 gap-2">
                        <label
                            v-for="allergen in allergenList"
                            :key="`exclude-${allergen}`"
                            class="flex items-center gap-2 text-sm"
                        >
                            <input
                                type="checkbox"
                                :checked="excludeAllergens.includes(allergen)"
                                @change="toggleExclude(allergen)"
                                class="accent-rose-600"
                            />
                            <span>{{ allergen }}</span>
                        </label>
                    </div>
                </div>
            </div>
        </section>

        <div
            v-if="paginationStore.allRecipesLoading"
            class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6"
        >
            <RecipeCardSkeleton v-for="skeleton in skeletonCards" :key="skeleton" />
        </div>
        <div
            v-else-if="paginationStore.allRecipes.length === 0"
            class="rounded-lg border border-dashed p-6 text-sm"
        >
            Nincs megjeleníthető recept.
        </div>
        <div
            v-else-if="filteredRecipes.length === 0"
            class="rounded-lg border border-dashed p-6 text-sm"
        >
            Nincs találat a kiválasztott allergén szűrőkkel.
        </div>
        <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <RecipeCard v-for="recipe in filteredRecipes" :key="recipe.id" :recipe="recipe" />
        </div>
        <RecipePagination
            :currentPage="paginationStore.allRecipesPagination.pageNumber"
            :totalPages="paginationStore.allRecipesPagination.pageCount"
            :hasNextPage="paginationStore.allRecipesPagination.hasNextPage"
            :hasPreviousPage="paginationStore.allRecipesPagination.hasPreviousPage"
            :disabled="paginationStore.allRecipesLoading"
            @pageChange="requestPage"
        />
    </main>
</template>
