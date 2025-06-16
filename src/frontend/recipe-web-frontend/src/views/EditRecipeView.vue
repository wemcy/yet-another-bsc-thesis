<script setup lang="ts">
import { useRecipeStore } from '@/stores/recipeStore'
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import RecipeEditForm from '@/components/recipe/RecipeEditForm.vue'

const route = useRoute()
const router = useRouter()
const recipeStore = useRecipeStore()
const recipe = computed(() => recipeStore.recipes.find((r) => r.id === route.params.id))

if (!recipe.value) {
    // ha nincs ilyen recept, mehet pl. vissza főoldalra
    router.push({ name: 'Home' })
}
</script>

<template>
    <div class="max-w-2xl mx-auto py-8">
        <h2 class="text-2xl font-bold mb-6">Recept szerkesztése</h2>
        <RecipeEditForm v-if="recipe" :recipe="recipe" />
        <div v-else>Recept nem található.</div>
    </div>
</template>
