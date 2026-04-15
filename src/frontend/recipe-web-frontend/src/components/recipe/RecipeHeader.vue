<template>
    <div class="mb-6">
        <h1 class="text-3xl font-bold mb-2">{{ title }}</h1>
        <p class="text-gray-700">{{ description }}</p>
        <router-link
            v-if="authorName && authorId"
            :to="{ name: 'PublicProfile', params: { id: authorId } }"
            class="inline-flex items-center gap-2 mt-3 px-3 py-1.5 bg-gray-100 rounded-full text-sm text-gray-700 hover:bg-blue-50 hover:text-blue-700 transition"
        >
            <img
                :src="`/api/profile/${authorId}/image?size=${ImageSize.Thumbnail}`"
                :alt="authorName"
                class="w-6 h-6 rounded-full object-cover"
                @error="($event.target as HTMLImageElement).style.display = 'none'"
            />
            <span class="font-medium">{{ authorName }}</span>
        </router-link>
        <div
            v-else-if="authorName"
            class="inline-flex items-center gap-2 mt-3 px-3 py-1.5 bg-gray-100 rounded-full text-sm text-gray-600"
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="w-5 h-5"
                viewBox="0 0 20 20"
                fill="currentColor"
            >
                <path
                    fill-rule="evenodd"
                    d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
                    clip-rule="evenodd"
                />
            </svg>
            <span class="font-medium">{{ authorName }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ImageSize } from 'recipe-api-client'

defineProps<{
    title: string
    description: string
    authorName?: string
    authorId: string
}>()
</script>
