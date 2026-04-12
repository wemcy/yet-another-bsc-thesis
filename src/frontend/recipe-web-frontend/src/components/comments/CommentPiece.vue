<template>
    <div class="border-b pb-4">
        <router-link
            v-if="authorId"
            :to="{ name: 'PublicProfile', params: { id: authorId } }"
            class="font-semibold text-blue-600 hover:underline"
        >
            {{ name }}
        </router-link>
        <p v-else class="font-semibold">{{ name }}</p>
        <p class="text-gray-700">{{ content }}</p>
        <p class="text-sm text-gray-400 mt-1">{{ date }}</p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    name: string
    authorId?: string
    content: string
    date: string
}>()
</script>
