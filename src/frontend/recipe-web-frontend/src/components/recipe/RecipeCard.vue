<template>
    <router-link
        :to="{ name: 'Recipe', params: { id: recipe.id } }"
        class="block bg-white shadow-md rounded-lg overflow-hidden hover:shadow-lg transition"
    >
        <img :src="recipe.image" :alt="recipe.title" class="w-full h-40 object-cover" />
        <div class="p-4">
            <h3 class="font-semibold text-lg mb-2">{{ recipe.title }}</h3>
        </div>
    </router-link>
</template>
<script setup lang="ts">
import type { Recipe } from '@/types/recipe/recipe'
import { defineProps } from 'vue'

defineProps<{
    recipe: Recipe
}>()
</script>
