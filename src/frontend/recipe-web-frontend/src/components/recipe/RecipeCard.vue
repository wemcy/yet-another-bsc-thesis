<template>
    <router-link
        :to="{ name: 'Recipe', params: { id: recipe.id } }"
        class="block bg-white shadow-md rounded-lg overflow-hidden hover:shadow-lg transition"
    >
        <img :src="recipe.image" :alt="recipe.title" class="w-full h-40 object-cover" />
        <div class="p-4">
            <h3 class="font-semibold text-lg mb-2">{{ recipe.title }}</h3>
            <div v-if="recipe.authorName" class="flex items-center gap-1.5 text-sm text-gray-500">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="w-4 h-4"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                >
                    <path
                        fill-rule="evenodd"
                        d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
                        clip-rule="evenodd"
                    />
                </svg>
                <span
                    v-if="recipe.authorId"
                    class="text-blue-600 hover:underline cursor-pointer"
                    @click.stop.prevent="goToProfile"
                >
                    {{ recipe.authorName }}
                </span>
                <span v-else>{{ recipe.authorName }}</span>
            </div>
        </div>
    </router-link>
</template>
<script setup lang="ts">
import type { Recipe } from '@/types/recipe/recipe'
import { useRouter } from 'vue-router'

const { recipe } = defineProps<{ recipe: Recipe }>()
const router = useRouter()

function goToProfile() {
    if (recipe.authorId) {
        router.push({ name: 'PublicProfile', params: { id: recipe.authorId } })
    }
}
</script>
