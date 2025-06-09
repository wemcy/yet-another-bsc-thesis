<template>
    <div class="flex items-center gap-1 text-yellow-500 text-xl">
        <span v-for="n in 5" :key="n">
            <span v-if="n <= rating">★</span>
            <span v-else>☆</span>
        </span>
        <span class="text-sm text-gray-500 ml-2">({{ rating }}/5)</span>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    rating: number
}>()
</script>
