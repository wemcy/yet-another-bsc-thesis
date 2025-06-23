<script setup lang="ts">
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
import type { Recipe } from '@/types/recipe/recipe'

const { recipes } = defineProps<{ recipes: Recipe[] }>()
</script>

<template>
    <div class="mt-10">
        <h3 class="text-xl font-bold mb-4">Saját receptjeim</h3>
        <RecipePagination :items="recipes" :perPage="8" v-slot="{ items }">
            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                <RecipeCard v-for="recipe in items" :key="recipe.id" :recipe="recipe" />
            </div>
        </RecipePagination>
    </div>
</template>
