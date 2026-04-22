<template>
    <section class="max-w-6xl mx-auto px-4 py-10">
        <div class="bg-white shadow-lg rounded-lg overflow-hidden flex flex-col md:flex-row">
            <img
                :src="buildRecipeImageUrl(recipe.image, recipe.imageRevision, ImageSize.Large)"
                :alt="recipe.title"
                class="w-full md:w-1/2 h-64 object-cover"
            />
            <div class="p-6 flex flex-col justify-center">
                <h2 class="text-2xl font-bold mb-2">{{ recipe.title }}</h2>
                <p class="text-gray-600 mb-4">{{ recipe.description }}</p>
                <div class="flex items-center gap-1 mb-4 text-yellow-500">
                    <svg
                        v-for="star in 5"
                        :key="star"
                        xmlns="http://www.w3.org/2000/svg"
                        class="w-5 h-5"
                        :class="
                            star <= Math.round(recipe.rating) ? 'fill-current' : 'text-gray-300'
                        "
                        viewBox="0 0 20 20"
                        fill="currentColor"
                    >
                        <path
                            d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.957a1 1 0 00.95.69h4.162c.969 0 1.371 1.24.588 1.81l-3.37 2.448a1 1 0 00-.364 1.118l1.287 3.957c.3.921-.755 1.688-1.54 1.118l-3.37-2.448a1 1 0 00-1.176 0l-3.37 2.448c-.784.57-1.838-.197-1.539-1.118l1.287-3.957a1 1 0 00-.364-1.118L2.065 9.384c-.783-.57-.38-1.81.588-1.81h4.162a1 1 0 00.95-.69l1.284-3.957z"
                        />
                    </svg>
                    <span class="ml-1 text-sm text-gray-600">{{ recipe.rating.toFixed(1) }}</span>
                </div>
                <router-link
                    :to="`/recipe/${recipe.id}`"
                    class="inline-block bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
                >
                    Megnézem
                </router-link>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
import type { Recipe } from '@/types/recipe/recipe'
import { buildRecipeImageUrl } from '@/utils/imageUrl'
import { ImageSize } from 'recipe-api-client'

defineProps<{
    recipe: Recipe
}>()
</script>
