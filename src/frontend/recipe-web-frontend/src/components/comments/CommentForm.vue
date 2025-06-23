<template>
    <div v-if="auth.currentUser">
        <form @submit.prevent="submit" class="mt-6 space-y-4">
            <!-- Név csak kiírva, nem szerkeszthető -->
            <input
                :value="auth.currentUser.name"
                type="text"
                class="w-full border rounded-lg px-4 py-2 bg-gray-100 text-gray-500 cursor-not-allowed"
                disabled
            />
            <textarea
                v-model="message"
                rows="4"
                placeholder="Írd meg a véleményed..."
                class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            ></textarea>
            <button
                type="submit"
                class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 transition disabled:opacity-50"
                :disabled="!message.trim()"
            >
                Hozzászólás küldése
            </button>
        </form>
    </div>
    <div v-else class="text-gray-500 italic">Jelentkezz be a hozzászóláshoz!</div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import type { CommentPayload } from '@/types/comments'
const auth = useAuthStore()

const emit = defineEmits<{
    (event: 'submit', payload: CommentPayload): void
}>()

const message = ref('')

function submit() {
    if (!message.value.trim()) return

    emit('submit', {
        name: auth.currentUser!.name,
        message: message.value.trim(),
    })

    message.value = ''
}
</script>
