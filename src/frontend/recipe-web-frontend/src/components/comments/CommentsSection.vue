<template>
    <section class="max-w-6xl mx-auto px-4 py-10 mt-10 border-t pt-6">
        <h2 class="text-xl font-semibold mb-4">Hozzászólások</h2>

        <CommentForm @submit="handleSubmitComment" />

        <p
            v-if="commentSubmitError"
            class="mt-3 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700"
        >
            {{ commentSubmitError }}
        </p>

        <div v-if="isCommentsLoading" class="space-y-6 mt-4">
            <CommentPieceSkeleton v-for="n in 3" :key="n" />
        </div>
        <div v-else-if="comments.length === 0" class="text-sm text-gray-500 py-2 mt-4">
            Még nincs hozzászólás ehhez a recepthez.
        </div>
        <div v-else class="space-y-6 mt-4">
            <CommentPiece
                v-for="comment in comments"
                :key="comment.id"
                :name="comment.authorName"
                :authorId="comment.authorId"
                :content="comment.content"
                :date="formatDate(comment.createdAt)"
            />
        </div>

        <RecipePagination
            :currentPage="pagination.pageNumber"
            :totalPages="pagination.pageCount"
            :hasNextPage="pagination.hasNextPage"
            :hasPreviousPage="pagination.hasPreviousPage"
            :disabled="isCommentsLoading"
            @pageChange="loadPage"
        />
    </section>
</template>

<script setup lang="ts">
import CommentPiece from './CommentPiece.vue'
import CommentPieceSkeleton from './CommentPieceSkeleton.vue'
import CommentForm from './CommentForm.vue'
import RecipePagination from '@/components/recipe/RecipePagination.vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { toErrorMessage } from '@/utils/identityErrors'
import { useRoute } from 'vue-router'
import { computed, ref } from 'vue'

const route = useRoute()
const recipeStore = useRecipeStore()
const recipeId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id[0] : id
})

const comments = computed(() => recipeStore.getCommentsByRecipeId(recipeId.value ?? ''))
const pagination = computed(() => recipeStore.getCommentsPaginationByRecipeId(recipeId.value ?? ''))
const isCommentsLoading = computed(() =>
    recipeStore.isCommentsLoadingByRecipeId(recipeId.value ?? ''),
)
const isCommentSubmitting = ref(false)
const commentSubmitError = ref<string | null>(null)

async function loadPage(pageNumber: number) {
    if (!recipeId.value) return
    await recipeStore.fetchRecipeCommentsPage(recipeId.value, pageNumber, pagination.value.pageSize)
}

async function handleSubmitComment(payload: { content: string }) {
    if (!recipeId.value || isCommentSubmitting.value) return

    commentSubmitError.value = null
    isCommentSubmitting.value = true
    try {
        await recipeStore.addRecipeComment(recipeId.value, payload.content)
        await loadPage(pagination.value.pageNumber)
    } catch (error) {
        commentSubmitError.value = await toErrorMessage(error)
    } finally {
        isCommentSubmitting.value = false
    }
}

function formatDate(value: Date): string {
    return value.toLocaleDateString('hu-HU', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
    })
}
</script>
