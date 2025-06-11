<template>
    <main class="max-w-6xl mx-auto p-6">
        <h1 class="text-2xl font-bold mb-6">Receptek</h1>

        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <RecipeCard v-for="recipe in paginatedRecipes" :key="recipe.id" :recipe="recipe" />
        </div>

        <div class="flex justify-center mt-8 space-x-2">
            <button
                @click="prevPage"
                :disabled="currentPage === 1"
                class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
            >
                Előző
            </button>
            <button
                @click="nextPage"
                :disabled="currentPage === totalPages"
                class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
            >
                Következő
            </button>
        </div>
    </main>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRecipeStore } from '@/stores/recipeStore'
import RecipeCard from '@/components/recipe/RecipeCard.vue'

const recipeStore = useRecipeStore()
const perPage = 8
const currentPage = ref(1)

const totalPages = computed(() => Math.ceil(recipeStore.recipes.length / perPage))

const paginatedRecipes = computed(() =>
    recipeStore.recipes.slice((currentPage.value - 1) * perPage, currentPage.value * perPage),
)

function prevPage() {
    if (currentPage.value > 1) currentPage.value--
}

function nextPage() {
    if (currentPage.value < totalPages.value) currentPage.value++
}
</script>
