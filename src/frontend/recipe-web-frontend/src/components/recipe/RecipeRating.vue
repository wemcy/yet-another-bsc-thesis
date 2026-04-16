<template>
    <div
        class="flex flex-wrap items-center gap-1.5 text-yellow-500 text-xl sm:text-2xl"
        :class="{ 'opacity-60': isSubmitting }"
    >
        <button
            v-for="n in 5"
            :key="n"
            type="button"
            class="cursor-pointer h-9 w-9 inline-flex items-center justify-center rounded-md hover:bg-yellow-50"
            :disabled="isSubmitting"
            :aria-label="`${n} csillag`"
            @click="rate(n)"
        >
            <span v-if="n <= localRating">★</span>
            <span v-else>☆</span>
        </button>
        <span class="text-sm text-gray-500 ml-2">({{ localRating }}/5)</span>
    </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useAuthStore } from '@/stores/authStore'

const props = withDefaults(
    defineProps<{
        rating: number
        isSubmitting?: boolean
    }>(),
    {
        isSubmitting: false,
    },
)

const emit = defineEmits<{
    (e: 'rate', value: number): void
}>()

const auth = useAuthStore()
const localRating = ref(props.rating)
const isSubmitting = ref(props.isSubmitting)

watch(
    () => props.rating,
    (newVal) => {
        localRating.value = newVal
    },
)

watch(
    () => props.isSubmitting,
    (newVal) => {
        isSubmitting.value = newVal
    },
)

function rate(value: number) {
    if (isSubmitting.value) {
        return
    }

    if (!auth.isLoggedIn) {
        alert('Csak bejelentkezve értékelhetsz!')
        return
    }
    localRating.value = value
    emit('rate', value)
}
</script>
