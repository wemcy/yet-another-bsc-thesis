<script setup lang="ts">
import CommentsSection from '@/components/comments/CommentsSection.vue'
import IngredientList from '@/components/recipe/IngredientList.vue'
import RecipeHeader from '@/components/recipe/RecipeHeader.vue'
import InstructionsList from '@/components/recipe/InstuctionList.vue'
import AllergenList from '@/components/recipe/AllergenList.vue'
import RecipeRating from '@/components/recipe/RecipeRating.vue'
import { useRoute, useRouter } from 'vue-router'
import { useRecipeStore } from '@/stores/recipeStore'
import { useAuthStore } from '@/stores/authStore'
import { ImageSize } from 'recipe-api-client'
import { computed, ref, watch } from 'vue'

const route = useRoute()
const router = useRouter()
const recipeStore = useRecipeStore()
const auth = useAuthStore()
const recipe = computed(() => recipeStore.getById(route.params.id as string))
const isRatingSubmitting = ref(false)

async function updateRating(newRating: number) {
    const idParam = route.params.id
    const id = Array.isArray(idParam) ? idParam[0] : idParam

    if (!id || isRatingSubmitting.value) {
        return
    }

    isRatingSubmitting.value = true
    try {
        await recipeStore.updateRating(id, newRating)
    } catch {
        alert('Nem sikerult elmenteni az ertekelest. Probald ujra!')
    } finally {
        isRatingSubmitting.value = false
    }
}

const isOwnRecipe = computed(
    () => recipe.value && auth.currentUser && recipe.value.authorId === auth.currentUser.id,
)

const canEditRecipe = computed(
    () => isOwnRecipe.value || (auth.currentUser?.roles?.includes('Admin') ?? false),
)

const deleteDialogOpen = ref(false)
const isDeleting = ref(false)

function handleDeleteRecipe() {
    deleteDialogOpen.value = true
}

async function confirmDeleteRecipe() {
    if (!recipe.value || isDeleting.value) return
    isDeleting.value = true
    try {
        await recipeStore.deleteRecipe(recipe.value.id)
        deleteDialogOpen.value = false
        router.push({ name: 'Home' })
    } catch {
        alert('Nem sikerült törölni a receptet. Próbáld újra!')
    } finally {
        isDeleting.value = false
    }
}

watch(
    () => route.params.id,
    (id) => {
        if (id) recipeStore.fetchRecipeById(id as string)
    },
    { immediate: true },
)
</script>

<template>
    <div>
        <main
            v-if="recipe"
            class="max-w-6xl mx-auto px-4 py-10 grid md:grid-cols-3 gap-10 text-gray-800"
        >
            <!-- Bal oszlop -->
            <div class="md:col-span-2 space-y-6">
                <RecipeHeader
                    :title="recipe.title"
                    :description="recipe.description"
                    :authorName="recipe.authorName"
                    :authorId="recipe.authorId"
                />

                <IngredientList :ingredients="recipe.ingredients" />
                <InstructionsList :steps="recipe.steps" />
            </div>

            <!-- Jobb oszlop -->
            <div class="space-y-6">
                <RecipeRating
                    :rating="recipe.rating"
                    :is-submitting="isRatingSubmitting"
                    @rate="updateRating"
                />
                <img
                    :src="`${recipe.image}?size=${ImageSize.Large}`"
                    alt="Image of the recipe"
                    class="w-full h-64 object-cover rounded shadow"
                />
                <AllergenList :allergens="recipe.allergens" />
            </div>
        </main>
        <div v-if="!recipe" class="text-center py-20 text-gray-500">A recept nem található. 🫤</div>

        <div
            v-if="recipe && canEditRecipe"
            class="max-w-6xl mx-auto px-4 mt-6 mb-4 flex justify-center gap-3"
        >
            <router-link
                :to="`/edit/${recipe.id}`"
                class="border border-blue-400 text-blue-600 bg-blue-50 px-4 py-2 rounded hover:bg-blue-100 transition flex items-center gap-2 cursor-pointer"
            >
                <span>✏️</span>
                <span>Szerkesztés</span>
            </router-link>
            <button
                type="button"
                class="border border-red-400 text-red-600 bg-red-50 px-4 py-2 rounded hover:bg-red-100 transition flex items-center gap-2 cursor-pointer"
                @click="handleDeleteRecipe"
            >
                <span>🗑️</span>
                <span>Törlés</span>
            </button>
        </div>

        <CommentsSection v-if="recipe" />

        <div
            v-if="deleteDialogOpen"
            class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
        >
            <div class="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
                <h2 class="text-lg font-semibold text-gray-900">Recept törlése</h2>
                <p class="mt-2 text-sm text-gray-600">
                    Biztosan törölni szeretnéd ezt a receptet? Ez a művelet nem visszavonható.
                </p>
                <div class="mt-5 flex justify-end gap-2">
                    <button
                        type="button"
                        :disabled="isDeleting"
                        class="rounded border px-4 py-2 hover:bg-gray-100 cursor-pointer disabled:opacity-50"
                        @click="deleteDialogOpen = false"
                    >
                        Mégse
                    </button>
                    <button
                        type="button"
                        :disabled="isDeleting"
                        class="rounded bg-red-600 px-4 py-2 text-white hover:bg-red-700 cursor-pointer disabled:opacity-50"
                        @click="confirmDeleteRecipe"
                    >
                        {{ isDeleting ? 'Törlés...' : 'Igen, törlöm' }}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
