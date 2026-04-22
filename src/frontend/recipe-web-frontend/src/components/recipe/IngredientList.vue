<template>
    <div class="mb-6">
        <h2 class="text-xl font-semibold mb-2">Összetevők</h2>
        <ul
            class="list-disc list-outside pl-5 space-y-1.5 text-sm sm:text-base text-gray-700 leading-relaxed"
        >
            <li v-for="(item, index) in ingredients" :key="index">
                {{ item.quantity }} {{ item.unitOfMeasurement }} {{ item.name }}
            </li>
        </ul>
    </div>
</template>

<script setup lang="ts">
import type { Ingredient } from '@/types/recipe/ingredient'

defineProps<{
    ingredients: Ingredient[]
}>()
</script>
