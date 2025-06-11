<template>
    <section class="max-w-6xl mx-auto px-4 py-10">
        <div class="bg-white shadow-lg rounded-lg overflow-hidden flex flex-col md:flex-row">
            <img
                :src="recipe.image"
                :alt="recipe.title"
                class="w-full md:w-1/2 h-64 object-cover"
            />
            <div class="p-6 flex flex-col justify-center">
                <h2 class="text-2xl font-bold mb-2">{{ recipe.title }}</h2>
                <p class="text-gray-600 mb-4">{{ recipe.description }}</p>
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

defineProps<{
    recipe: Recipe
}>()
</script>
