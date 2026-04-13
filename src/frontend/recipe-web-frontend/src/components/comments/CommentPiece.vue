<template>
    <div class="border-b pb-4">
        <div class="flex items-center justify-between">
            <div class="flex items-center gap-2">
                <router-link
                    v-if="authorId"
                    :to="{ name: 'PublicProfile', params: { id: authorId } }"
                    class="font-semibold text-blue-600 hover:underline"
                >
                    {{ name }}
                </router-link>
                <p v-else class="font-semibold">{{ name }}</p>
                <span
                    v-if="isRecipeAuthor"
                    class="text-xs font-medium bg-blue-100 text-blue-700 px-2 py-0.5 rounded-full"
                >
                    Szerző
                </span>
            </div>
            <button
                v-if="canDelete"
                class="text-red-500 hover:text-red-700 text-sm transition"
                @click="$emit('delete')"
            >
                Törlés
            </button>
        </div>
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
    canDelete?: boolean
    isRecipeAuthor?: boolean
}>()

defineEmits<{
    delete: []
}>()
</script>
