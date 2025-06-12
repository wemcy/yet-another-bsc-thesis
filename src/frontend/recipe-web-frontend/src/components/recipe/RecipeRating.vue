<template>
    <div class="flex items-center gap-1 text-yellow-500 text-2xl">
        <span v-for="n in 5" :key="n" class="cursor-pointer" @click="rate(n)">
            <span v-if="n <= localRating">★</span>
            <span v-else>☆</span>
        </span>
        <span class="text-sm text-gray-500 ml-2">({{ localRating }}/5)</span>
    </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useAuthStore } from '@/stores/authStore'

const props = defineProps<{
    rating: number
}>()

const emit = defineEmits<{
    (e: 'rate', value: number): void
}>()

const auth = useAuthStore()
const localRating = ref(props.rating)

watch(
    () => props.rating,
    (newVal) => {
        localRating.value = newVal
    },
)

function rate(value: number) {
    if (!auth.isLoggedIn) {
        alert('Csak bejelentkezve értékelhetsz!')
        return
    }
    localRating.value = value
    emit('rate', value)
}
</script>
