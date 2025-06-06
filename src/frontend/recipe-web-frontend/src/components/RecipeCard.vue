<template>
    <div class="bg-white shadow-md rounded-lg overflow-hidden hover:shadow-lg transition">
        <img :src="image" :alt="title" class="w-full h-40 object-cover" />
        <div class="p-4">
            <h3 class="font-semibold text-lg mb-2">{{ title }}</h3>
            <p class="text-sm text-gray-600">Allergén: {{ allergens }}</p>
        </div>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    title: string
    image: string
    allergens: string
}>()
</script>
