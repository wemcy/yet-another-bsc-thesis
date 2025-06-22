<script setup lang="ts">
import type { Recipe } from '@/types/recipe/recipe'
import { computed, ref, watch } from 'vue'

const props = defineProps<{
    items: Recipe[]
    perPage?: number
}>()

const currentPage = ref(1)
const perPage = computed(() => props.perPage || 8)
const totalPages = computed(() => Math.ceil(props.items.length / perPage.value))

const paginatedItems = computed(() =>
    props.items.slice((currentPage.value - 1) * perPage.value, currentPage.value * perPage.value),
)

// Lapozó nullázódik, ha új lista jön (pl. keresés, filter)
watch(
    () => props.items,
    () => {
        currentPage.value = 1
    },
)

defineExpose({ currentPage, totalPages, paginatedItems })
</script>

<template>
    <div>
        <slot :items="paginatedItems" />
        <div class="flex justify-center mt-8 space-x-2">
            <button
                @click="currentPage--"
                :disabled="currentPage === 1"
                class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
            >
                Előző
            </button>
            <span
                v-for="n in totalPages"
                :key="n"
                class="px-3 py-2 rounded"
                :class="n === currentPage ? 'bg-blue-600 text-white' : 'bg-gray-200'"
                @click="currentPage = n"
                style="cursor: pointer"
                >{{ n }}</span
            >
            <button
                @click="currentPage++"
                :disabled="currentPage === totalPages"
                class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
            >
                Következő
            </button>
        </div>
    </div>
</template>
