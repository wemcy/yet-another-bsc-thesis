<template>
    <form @submit.prevent="submit" class="mt-6 space-y-4">
        <textarea
            v-model="message"
            rows="4"
            placeholder="Írd meg a véleményed..."
            class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
        ></textarea>
        <button
            type="submit"
            class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 transition"
        >
            Hozzászólás küldése
        </button>
    </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const message = ref('')

function submit() {
    if (message.value.trim()) {
        // Később emit vagy store művelet jöhet ide
        console.log('Hozzászólás elküldve:', message.value)
        message.value = ''
    }
}
</script>
