<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
    currentPage: number
    totalPages: number
    hasNextPage: boolean
    hasPreviousPage: boolean
    disabled?: boolean
}>()

const emit = defineEmits<{
    pageChange: [pageNumber: number]
}>()

const pages = computed<(number | 'ellipsis')[]>(() => {
    if (props.totalPages <= 0) return []

    const lastPage = props.totalPages - 1
    const current = Math.min(Math.max(props.currentPage, 0), lastPage)

    if (props.totalPages <= 7) {
        return Array.from({ length: props.totalPages }, (_, i) => i)
    }

    const nearStart = current <= 3
    const nearEnd = current >= lastPage - 3

    if (nearStart) {
        return [0, 1, 2, 3, 4, 'ellipsis', lastPage]
    }

    if (nearEnd) {
        return [0, 'ellipsis', lastPage - 4, lastPage - 3, lastPage - 2, lastPage - 1, lastPage]
    }

    return [0, 'ellipsis', current - 1, current, current + 1, 'ellipsis', lastPage]
})

function setPage(pageNumber: number) {
    if (props.disabled) return
    if (pageNumber < 0 || pageNumber >= props.totalPages) return
    if (pageNumber === props.currentPage) return
    emit('pageChange', pageNumber)
}
</script>

<template>
    <div v-if="totalPages > 0" class="flex justify-center mt-8 items-center gap-2">
        <button
            @click="setPage(currentPage - 1)"
            :disabled="disabled || !hasPreviousPage"
            class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
        >
            Előző
        </button>
        <template v-for="(item, idx) in pages" :key="`${item}-${idx}`">
            <button
                v-if="item !== 'ellipsis'"
                @click="setPage(item)"
                :disabled="disabled"
                class="px-3 py-2 rounded min-w-10"
                :class="item === currentPage ? 'bg-blue-600 text-white' : 'bg-gray-200'"
            >
                {{ item + 1 }}
            </button>
            <span v-else class="px-2 text-gray-500">...</span>
        </template>
        <button
            @click="setPage(currentPage + 1)"
            :disabled="disabled || !hasNextPage"
            class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
        >
            Következő
        </button>
    </div>
</template>
