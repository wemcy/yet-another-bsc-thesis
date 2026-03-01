<template>
    <div class="border-b pb-4">
        <p class="font-semibold">{{ name }}</p>
        <p class="text-gray-700">{{ content }}</p>
        <p class="text-sm text-gray-400 mt-1">{{ date }}</p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    name: string
    content: string
    date: string
}>()
</script>
