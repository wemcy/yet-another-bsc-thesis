<script setup lang="ts">
import { useRecipePaginationStore } from '@/stores/recipePaginationStore'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import { MapApiAllergenToEnum, MapEnumToApiAllergen } from '@/types/recipe/allergen.mappers'
import { AllergenEnum, allergenList } from '@/types/recipe/allergens'
import { computed, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { Allergen as ApiAllergen } from 'recipe-api-client'

const paginationStore = useRecipePaginationStore()
const pageSize = 27
const skeletonCards = computed(() => Array.from({ length: pageSize }, (_, i) => i))
const uiAllergenList = allergenList as readonly AllergenEnum[]
const route = useRoute()
const router = useRouter()
const includeAllergens = ref<AllergenEnum[]>([])
const excludeAllergens = ref<AllergenEnum[]>([])
const appliedIncludeAllergens = ref<AllergenEnum[]>([])
const appliedExcludeAllergens = ref<AllergenEnum[]>([])

const validApiAllergens = new Set<ApiAllergen>([
    'GLUTEN',
    'CRUSTACEANS',
    'EGGS',
    'FISH',
    'PEANUTS',
    'SOYBEANS',
    'MILK',
    'NUTS',
    'CELERY',
    'MUSTARD',
    'SESAMESEEDS',
    'SULPHURDIOXIDE',
    'LUPIN',
    'MOLLUSCS',
])

const hasPendingFilterChanges = computed(() => {
    const includeChanged =
        includeAllergens.value.length !== appliedIncludeAllergens.value.length ||
        includeAllergens.value.some((item) => !appliedIncludeAllergens.value.includes(item))
    const excludeChanged =
        excludeAllergens.value.length !== appliedExcludeAllergens.value.length ||
        excludeAllergens.value.some((item) => !appliedExcludeAllergens.value.includes(item))

    return includeChanged || excludeChanged
})

const hasAppliedFilters = computed(
    () => appliedIncludeAllergens.value.length > 0 || appliedExcludeAllergens.value.length > 0,
)

function parseAllergenQueryParam(param: unknown): AllergenEnum[] {
    if (typeof param !== 'string' || param.trim() === '') return []

    const uniqueApiValues = [...new Set(param.split(',').map((value) => value.trim()))].filter(
        (value): value is ApiAllergen => validApiAllergens.has(value as ApiAllergen),
    )

    return uniqueApiValues.map((value) => MapApiAllergenToEnum(value))
}

function getPageFromQuery() {
    const value = route.query.page
    const raw = typeof value === 'string' ? Number.parseInt(value, 10) : NaN
    if (Number.isNaN(raw) || raw < 1) return 0
    return raw - 1
}

function syncFiltersFromQuery() {
    const includeFromQuery = parseAllergenQueryParam(route.query.include)
    const excludeFromQuery = parseAllergenQueryParam(route.query.exclude).filter(
        (item) => !includeFromQuery.includes(item),
    )

    includeAllergens.value = [...includeFromQuery]
    excludeAllergens.value = [...excludeFromQuery]
    appliedIncludeAllergens.value = includeFromQuery
    appliedExcludeAllergens.value = excludeFromQuery
}

async function loadPage(pageNumber: number) {
    const includeApiAllergens = appliedIncludeAllergens.value.map((item) =>
        MapEnumToApiAllergen(item),
    )
    const excludeApiAllergens = appliedExcludeAllergens.value.map((item) =>
        MapEnumToApiAllergen(item),
    )

    await paginationStore.loadAllRecipesPage(
        pageNumber,
        pageSize,
        includeApiAllergens,
        excludeApiAllergens,
    )
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
    const includeApiValues = appliedIncludeAllergens.value.map((item) => MapEnumToApiAllergen(item))
    const excludeApiValues = appliedExcludeAllergens.value.map((item) => MapEnumToApiAllergen(item))

    await router.replace({
        query: {
            ...route.query,
            page: '1',
            include: includeApiValues.length > 0 ? includeApiValues.join(',') : undefined,
            exclude: excludeApiValues.length > 0 ? excludeApiValues.join(',') : undefined,
        },
    })
}

async function toggleInclude(allergen: AllergenEnum) {
    if (includeAllergens.value.includes(allergen)) {
        includeAllergens.value = includeAllergens.value.filter((item) => item !== allergen)
    } else {
        includeAllergens.value = [...includeAllergens.value, allergen]
        excludeAllergens.value = excludeAllergens.value.filter((item) => item !== allergen)
    }
}

async function toggleExclude(allergen: AllergenEnum) {
    if (excludeAllergens.value.includes(allergen)) {
        excludeAllergens.value = excludeAllergens.value.filter((item) => item !== allergen)
    } else {
        excludeAllergens.value = [...excludeAllergens.value, allergen]
        includeAllergens.value = includeAllergens.value.filter((item) => item !== allergen)
    }
}

async function clearAllergenFilters() {
    includeAllergens.value = []
    excludeAllergens.value = []
}

async function applyFilters() {
    appliedIncludeAllergens.value = [...includeAllergens.value]
    appliedExcludeAllergens.value = [...excludeAllergens.value]
    await updateAllergenQuery()
}

watch(
    () => [route.query.page, route.query.include, route.query.exclude],
    async () => {
        syncFiltersFromQuery()
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
                            v-for="allergen in uiAllergenList"
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
                            v-for="allergen in uiAllergenList"
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

            <div class="mt-4 flex justify-end">
                <button
                    type="button"
                    class="bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
                    :disabled="!hasPendingFilterChanges || paginationStore.allRecipesLoading"
                    @click="applyFilters"
                >
                    Szűrő alkalmazása
                </button>
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
            {{
                hasAppliedFilters
                    ? 'Nincs találat a kiválasztott allergén szűrőkkel.'
                    : 'Nincs megjeleníthető recept.'
            }}
        </div>
        <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <RecipeCard
                v-for="recipe in paginationStore.allRecipes"
                :key="recipe.id"
                :recipe="recipe"
            />
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
