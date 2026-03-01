<template>
    <section class="max-w-6xl mx-auto px-4 py-10 mt-10 border-t pt-6">
        <h2 class="text-xl font-semibold mb-4">Hozzászólások</h2>

        <div class="space-y-6">
            <Comment
                v-for="(comment, index) in comments"
                :key="index"
                :name="comment.authorId"
                :content="comment.content"
                :date="
                    comment.createdAt.toLocaleDateString('hu-HU', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                    })
                "
            />
        </div>

        <!-- <CommentForm @submit="addComment" /> -->
    </section>
</template>

<script setup lang="ts">
import Comment from './CommentPiece.vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { useRoute } from 'vue-router'
import { computed } from 'vue'

const route = useRoute()
const recipeStore = useRecipeStore()
const comments = computed(() => recipeStore.getCommentsByRecipeId(route.params.id as string))

// const comments = function addComment({ name, message }: { name: string; message: string }) {
//     const newComment: CommentItem = {
//         name,
//         text: message,
//         date: new Date().toLocaleDateString('hu-HU', {
//             year: 'numeric',
//             month: '2-digit',
//             day: '2-digit',
//         }),
//     }
//     comments.value.push(newComment)
// }
</script>
