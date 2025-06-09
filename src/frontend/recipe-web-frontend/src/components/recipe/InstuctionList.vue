<template>
    <div class="mb-6">
        <h2 class="text-xl font-semibold mb-2">Elkészítés</h2>
        <ol class="list-decimal list-inside space-y-1 text-gray-700">
            <li v-for="(step, index) in steps" :key="index">
                {{ step }}
            </li>
        </ol>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    steps: string[]
}>()
</script>
