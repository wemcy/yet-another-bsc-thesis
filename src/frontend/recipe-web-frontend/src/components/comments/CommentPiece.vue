<template>
    <div class="border-b pb-4">
        <div class="flex items-start justify-between gap-4">
            <div class="flex items-start gap-3">
                <router-link
                    v-if="authorId"
                    :to="{ name: 'PublicProfile', params: { id: authorId } }"
                    class="shrink-0"
                >
                    <ProfileAvatar :src="avatarUrl" :alt="name" size="sm" />
                </router-link>
                <ProfileAvatar v-else :src="undefined" :alt="name" size="sm" class="shrink-0" />

                <div>
                    <div class="flex flex-wrap items-center gap-2">
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
                            class="rounded-full bg-blue-100 px-2 py-0.5 text-xs font-medium text-blue-700"
                        >
                            Szerző
                        </span>
                    </div>
                    <p class="mt-1 text-gray-700">{{ content }}</p>
                    <p class="mt-1 text-sm text-gray-400">{{ date }}</p>
                </div>
            </div>
            <button
                v-if="canDelete"
                type="button"
                class="inline-flex items-center gap-1.5 rounded-full border border-red-200 bg-white px-3 py-1.5 text-sm font-medium text-red-600 shadow-sm transition hover:-translate-y-0.5 hover:border-red-300 hover:bg-red-50 hover:text-red-700 focus:outline-none focus:ring-2 focus:ring-red-200"
                @click="$emit('delete')"
            >
                <span aria-hidden="true" class="text-base leading-none">×</span>
                Törlés
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import ProfileAvatar from '@/components/profile/ProfileAvatar.vue'
import { ImageSize } from 'recipe-api-client'
import { computed } from 'vue'

const props = defineProps<{
    name: string
    authorId?: string
    content: string
    date: string
    canDelete?: boolean
    isRecipeAuthor?: boolean
}>()

const avatarUrl = computed(() =>
    props.authorId ? `/api/profile/${props.authorId}/image?size=${ImageSize.Thumbnail}` : undefined,
)

defineEmits<{
    delete: []
}>()
</script>
