<template>
    <div class="mb-6">
        <h2 class="text-xl font-semibold mb-2">Összetevők</h2>
        <ul class="list-disc list-inside space-y-1 text-gray-700">
            <li v-for="(item, index) in ingredients" :key="index">
                {{ item.amount }} {{ item.unit }} {{ item.name }}
            </li>
        </ul>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    ingredients: {
        name: string
        amount: number | string
        unit: string
    }[]
}>()
</script>
