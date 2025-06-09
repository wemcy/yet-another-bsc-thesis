<template>
    <div class="mb-6">
        <h1 class="text-3xl font-bold mb-2">{{ title }}</h1>
        <p class="text-gray-700">{{ description }}</p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    title: string
    description: string
}>()
</script>
