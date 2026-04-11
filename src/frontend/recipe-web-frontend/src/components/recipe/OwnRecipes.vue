<script setup lang="ts">
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import type { PaginationState, Recipe } from '@/types/recipe/recipe'
import { computed } from 'vue'

const emit = defineEmits<{
    pageChange: [pageNumber: number]
}>()

const props = defineProps<{
    recipes: Recipe[]
    pagination: PaginationState
    loading?: boolean
}>()

const skeletonCards = computed(() =>
    Array.from({ length: props.pagination.pageSize || 27 }, (_, i) => i),
)
</script>

<template>
    <div class="mt-10">
        <h3 class="text-xl font-bold mb-4">Saját receptjeim</h3>
        <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <RecipeCardSkeleton v-for="skeleton in skeletonCards" :key="skeleton" />
        </div>
        <div v-else-if="recipes.length === 0" class="rounded-lg border border-dashed p-6 text-sm">
            Még nincs saját recepted.
        </div>
        <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <RecipeCard v-for="recipe in recipes" :key="recipe.id" :recipe="recipe" />
        </div>
        <RecipePagination
            :currentPage="pagination.pageNumber"
            :totalPages="pagination.pageCount"
            :hasNextPage="pagination.hasNextPage"
            :hasPreviousPage="pagination.hasPreviousPage"
            :disabled="loading"
            @pageChange="(pageNumber) => emit('pageChange', pageNumber)"
        />
    </div>
</template>
