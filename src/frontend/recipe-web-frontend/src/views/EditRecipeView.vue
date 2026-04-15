<script setup lang="ts">
import { useRecipeStore } from '@/stores/recipeStore'
import { computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import RecipeEditForm from '@/components/recipe/RecipeEditForm.vue'

const route = useRoute()
const router = useRouter()
const recipeStore = useRecipeStore()
const recipe = computed(() => recipeStore.recipes.find((r) => r.id === route.params.id))

watch(
    recipe,
    (val) => {
        if (!val) router.push({ name: 'Home' })
    },
    { immediate: true },
)
</script>

<template>
    <main class="max-w-4xl mx-auto px-4 py-10 text-gray-800">
        <h1 class="text-3xl font-bold mb-6 text-center">Recept szerkesztése</h1>
        <RecipeEditForm v-if="recipe" :recipe="recipe!" />
        <div v-else>Recept nem található.</div>
    </main>
</template>
