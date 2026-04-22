<template>
    <div class="mb-6">
        <h2 class="text-xl font-semibold mb-2">Elkészítés</h2>
        <ol
            class="list-decimal list-outside pl-5 space-y-2 text-sm sm:text-base text-gray-700 leading-relaxed"
        >
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
