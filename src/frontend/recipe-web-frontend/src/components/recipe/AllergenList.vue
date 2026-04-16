<template>
    <div class="mb-6">
        <h2 class="text-xl font-semibold mb-2">Allergének</h2>
        <ul
            class="list-disc list-outside pl-5 text-sm sm:text-base text-gray-700 space-y-1.5 leading-relaxed"
        >
            <li v-for="(allergen, index) in allergens" :key="index">{{ allergen }}</li>
        </ul>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    allergens: string[]
}>()
</script>
