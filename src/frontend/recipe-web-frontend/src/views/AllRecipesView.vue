<script setup lang="ts">
import { useRecipePaginationStore } from '@/stores/recipePaginationStore'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import { computed, onMounted } from 'vue'

const paginationStore = useRecipePaginationStore()
const pageSize = 27
const skeletonCards = computed(() => Array.from({ length: pageSize }, (_, i) => i))

async function loadPage(pageNumber: number) {
    await paginationStore.loadAllRecipesPage(pageNumber, pageSize)
}

onMounted(() => {
    loadPage(0)
})
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
            @pageChange="loadPage"
        />
    </main>
</template>
