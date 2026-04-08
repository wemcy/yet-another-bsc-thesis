<script setup lang="ts">
import { useRecipePaginationStore } from '@/stores/recipePaginationStore'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import { computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const paginationStore = useRecipePaginationStore()
const pageSize = 27
const skeletonCards = computed(() => Array.from({ length: pageSize }, (_, i) => i))
const route = useRoute()
const router = useRouter()

function getPageFromQuery() {
    const value = route.query.page
    const raw = typeof value === 'string' ? Number.parseInt(value, 10) : NaN
    if (Number.isNaN(raw) || raw < 1) return 0
    return raw - 1
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
