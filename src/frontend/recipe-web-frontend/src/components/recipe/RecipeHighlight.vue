<template>
    <section class="max-w-6xl mx-auto px-4 py-10">
        <div class="bg-white shadow-lg rounded-lg overflow-hidden flex flex-col md:flex-row">
            <img :src="image" :alt="title" class="w-full md:w-1/2 h-64 object-cover" />
            <div class="p-6 flex flex-col justify-center">
                <h2 class="text-2xl font-bold mb-2">{{ title }}</h2>
                <p class="text-gray-600 mb-4">{{ description }}</p>
                <button
                    class="self-start bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
                >
                    Megnézem
                </button>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
defineProps<{
    title: string
    description: string
    image: string
}>()
</script>
