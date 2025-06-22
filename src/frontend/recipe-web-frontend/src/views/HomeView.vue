<script setup lang="ts">
import RecipeHighlight from '@/components/recipe/RecipeHighlight.vue'

import RecipeCard from '@/components/recipe/RecipeCard.vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { computed } from 'vue'

const recipeStore = useRecipeStore()
const featured = computed(() => recipeStore.featuredRecipe)
</script>

<template>
    <main class="text-gray-800">
        <section class="text-center py-20 bg-white px-4">
            <h1 class="text-4xl font-bold mb-4">Fedezd fel a legjobb házias recepteket</h1>
            <p class="text-lg text-gray-600 mb-6">Egyszerűen kereshető, allergénszűrővel</p>
            <router-link
                to="/recipes"
                class="bg-blue-600 text-white px-6 py-3 rounded-lg text-lg hover:bg-blue-700 transition"
            >
                Böngészés a receptek között
            </router-link>
        </section>

        <section class="max-w-4xl mx-auto px-4 py-10">
            <input
                type="text"
                placeholder="Keresés..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </section>
        <RecipeHighlight v-if="featured" :recipe="featured" />
        <section class="max-w-6xl mx-auto px-4 py-12">
            <h2 class="text-2xl font-bold mb-6 text-center">Kiemelt receptek</h2>
            <div class="grid gap-6 md:grid-cols-3">
                <RecipeCard
                    v-for="recipe in recipeStore.recipes"
                    :key="recipe.id"
                    :recipe="recipe"
                />
            </div>
        </section>
    </main>
</template>
