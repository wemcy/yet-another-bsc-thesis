<template>
    <section class="max-w-6xl mx-auto px-4 py-10 mt-10 border-t pt-6">
        <h2 class="text-xl font-semibold mb-4">Hozzászólások</h2>

        <div class="space-y-6">
            <Comment
                v-for="(comment, index) in comments"
                :key="index"
                :name="comment.name"
                :text="comment.text"
                :date="comment.date"
            />
        </div>

        <CommentForm @submit="addComment" />
    </section>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import Comment from './CommentPiece.vue'
import CommentForm from './CommentForm.vue'

type CommentItem = {
    name: string
    text: string
    date: string
}

const comments = ref<CommentItem[]>([
    {
        name: 'Kiss Péter',
        text: 'Nagyon jó recept, múlt héten is elkészítettem!',
        date: '2024. 11. 08.',
    },
    {
        name: 'Nagy Eszter',
        text: 'Kicsit több sóval még finomabb lett 😄',
        date: '2024. 12. 01.',
    },
])

function addComment(text: string) {
    comments.value.push({
        name: 'Vendég', // később: bejelentkezett felhasználóból
        text,
        date: new Date().toLocaleDateString('hu-HU'),
    })
}
</script>
