<script setup lang="ts">
import RecipeHighlight from '@/components/recipe/RecipeHighlight.vue'
import RecipeHighlightSkeleton from '@/components/recipe/RecipeHighlightSkeleton.vue'

import RecipeCard from '@/components/recipe/RecipeCard.vue'
import RecipeCardSkeleton from '@/components/recipe/RecipeCardSkeleton.vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { computed } from 'vue'
import { onMounted } from 'vue'

const recipeStore = useRecipeStore()
const featured = computed(() => recipeStore.featuredRecipe)
const showcaseRecipes = computed(() => recipeStore.showcaseRecipes)
const showcaseSkeletons = Array.from({ length: 9 }, (_, i) => i)

onMounted(async () => {
    await Promise.all([recipeStore.fetchFeaturedRecipe(), recipeStore.fetchShowcaseRecipes()])
})
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
        <RecipeHighlightSkeleton v-if="recipeStore.featuredRecipeLoading" />
        <RecipeHighlight v-else-if="featured" :recipe="featured" />
        <section
            v-else-if="recipeStore.featuredRecipeLoaded"
            class="mx-auto max-w-6xl px-4 py-10"
            data-cy="featured-recipe-error"
        >
            <div
                class="rounded-lg border border-dashed border-gray-300 bg-white p-8 text-center text-gray-600"
            >
                <p class="mb-2 text-4xl" aria-hidden="true">☹</p>
                <h2 class="text-xl font-semibold text-gray-800">
                    Most nincs megjeleníthető recept.
                </h2>
                <p class="mt-2 text-sm">
                    {{ recipeStore.featuredRecipeError ?? 'A nap receptje jelenleg nem elérhető.' }}
                </p>
            </div>
        </section>
        <section class="max-w-6xl mx-auto px-4 py-12">
            <h2 class="text-2xl font-bold mb-6 text-center">Kiemelt receptek</h2>
            <div v-if="recipeStore.showcaseRecipesLoading" class="grid gap-6 md:grid-cols-3">
                <RecipeCardSkeleton v-for="skeleton in showcaseSkeletons" :key="skeleton" />
            </div>
            <div
                v-else-if="
                    recipeStore.showcaseRecipesLoaded &&
                    (recipeStore.showcaseRecipesError || showcaseRecipes.length === 0)
                "
                class="rounded-lg border border-dashed border-gray-300 bg-white p-8 text-center text-gray-600"
                data-cy="showcase-recipes-error"
            >
                <p class="mb-2 text-4xl" aria-hidden="true">☹</p>
                <p class="font-semibold text-gray-800">Nem sikerült kiemelt recepteket mutatni.</p>
                <p class="mt-2 text-sm">
                    {{
                        recipeStore.showcaseRecipesError ??
                        'Jelenleg nincs elérhető kiemelt recept.'
                    }}
                </p>
            </div>
            <div v-else class="grid gap-6 md:grid-cols-3">
                <RecipeCard v-for="recipe in showcaseRecipes" :key="recipe.id" :recipe="recipe" />
            </div>
        </section>
    </main>
</template>
