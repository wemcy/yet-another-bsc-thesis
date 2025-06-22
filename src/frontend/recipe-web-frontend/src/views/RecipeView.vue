<script setup lang="ts">
import CommentsSection from '@/components/comments/CommentsSection.vue'
import IngredientList from '@/components/recipe/IngredientList.vue'
import RecipeHeader from '@/components/recipe/RecipeHeader.vue'
import InstructionsList from '@/components/recipe/InstuctionList.vue'
import AllergenList from '@/components/recipe/AllergenList.vue'
import RecipeRating from '@/components/recipe/RecipeRating.vue'
import { useRoute } from 'vue-router'
import { useRecipeStore } from '@/stores/recipeStore'
import { useAuthStore } from '@/stores/authStore'
import { computed } from 'vue'

const route = useRoute()
const recipeStore = useRecipeStore()
const auth = useAuthStore()
const recipe = recipeStore.getById(route.params.id as string)

function updateRating(newRating: number) {
    const id = route.params.id[0]
    recipeStore.updateRating(id, newRating)
}

const isOwnRecipe = computed(
    () => recipe && auth.currentUser && recipe.authorId === auth.currentUser.id,
)
</script>

<template>
    <main
        v-if="recipe"
        class="max-w-6xl mx-auto px-4 py-10 grid md:grid-cols-3 gap-10 text-gray-800"
    >
        <!-- Bal oszlop -->
        <div class="md:col-span-2 space-y-6">
            <RecipeHeader :title="recipe.title" :description="recipe.description" />
            <router-link
                v-if="isOwnRecipe"
                :to="`/edit/${recipe.id}`"
                class="ml-2 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
            >
                ✏️ Szerkesztés
            </router-link>
            <IngredientList :ingredients="recipe.ingredients" />
            <InstructionsList :steps="recipe.steps" />
        </div>

        <!-- Jobb oszlop -->
        <div class="space-y-6">
            <RecipeRating :rating="recipe.rating" @rate="updateRating" />
            <img
                :src="recipe.image"
                alt="Image of the recipe"
                class="w-full h-64 object-cover rounded shadow"
            />
            <AllergenList :allergens="recipe.allergens" />
        </div>
    </main>
    <div v-else class="text-center py-20 text-gray-500">A recept nem található. 🫤</div>
    <CommentsSection v-if="recipe" />
</template>
