<template>
    <form @submit.prevent="submit" class="space-y-6">
        <!-- Title -->
        <div>
            <label class="block font-semibold mb-1">Recept neve</label>
            <input
                v-model="title"
                type="text"
                class="w-full border rounded px-4 py-2 bg-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
            <p v-if="errors.title" class="text-red-600 text-sm mt-1">{{ errors.title }}</p>
        </div>

        <!-- Description -->
        <div>
            <label class="block font-semibold mb-1">Leírás</label>
            <textarea
                v-model="description"
                class="w-full border rounded px-4 py-2 bg-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400"
                rows="3"
            ></textarea>
            <p v-if="errors.description" class="text-red-600 text-sm mt-1">
                {{ errors.description }}
            </p>
        </div>

        <!-- Ingredients -->
        <div>
            <label class="block font-semibold mb-2">Hozzávalók</label>
            <div v-for="(ingredient, index) in ingredients" :key="index" class="flex gap-2 mb-2">
                <input
                    v-model.number="ingredient.quantity"
                    type="number"
                    step="any"
                    placeholder="Mennyiség"
                    class="w-1/4 border rounded px-2 py-1 bg-white shadow-sm"
                />
                <input
                    v-model="ingredient.unitOfMeasurement"
                    type="text"
                    placeholder="Egység"
                    class="w-1/4 border rounded px-2 py-1 bg-white shadow-sm"
                />
                <input
                    v-model="ingredient.name"
                    type="text"
                    placeholder="Hozzávaló"
                    class="w-full border rounded px-2 py-1 bg-white shadow-sm"
                />
                <button type="button" @click="removeIngredient(index)" class="text-red-500">
                    ✕
                </button>
            </div>
            <button
                type="button"
                @click="addIngredient"
                class="text-blue-600 text-sm hover:underline"
            >
                + Hozzávaló hozzáadása
            </button>
            <p v-if="errors.ingredients" class="text-red-600 text-sm mt-1">
                {{ errors.ingredients }}
            </p>
        </div>

        <!-- Steps -->
        <div>
            <label class="block font-semibold mb-2">Elkészítési lépések</label>
            <div v-for="(step, index) in steps" :key="index" class="flex gap-2 mb-2">
                <textarea
                    v-model="steps[index]"
                    rows="2"
                    class="w-full border rounded px-2 py-1 bg-white shadow-sm"
                ></textarea>
                <button type="button" @click="removeStep(index)" class="text-red-500">✕</button>
            </div>
            <button type="button" @click="addStep" class="text-blue-600 text-sm hover:underline">
                + Lépés hozzáadása
            </button>
            <p v-if="errors.steps" class="text-red-600 text-sm mt-1">{{ errors.steps }}</p>
        </div>

        <!-- Allergens -->
        <div>
            <label class="block font-semibold mb-2">Allergének</label>
            <div class="grid grid-cols-2 sm:grid-cols-3 gap-2 text-sm text-gray-700">
                <label v-for="item in allergenOptions" :key="item" class="flex items-center gap-2">
                    <input
                        type="checkbox"
                        :value="item"
                        v-model="selectedAllergens"
                        class="accent-blue-600 bg-white shadow-sm"
                    />
                    {{ item }}
                </label>
            </div>
        </div>

        <!-- Image upload -->
        <div>
            <label class="block font-semibold mb-2">Kép feltöltése</label>
            <input
                type="file"
                accept="image/*"
                @change="handleImageChange"
                class="bg-white shadow-sm"
            />
            <div v-if="imageUrl" class="mt-2">
                <img :src="imageUrl" alt="Preview" class="w-64 h-40 object-cover rounded shadow" />
            </div>
        </div>

        <!-- Actions -->
        <p v-if="submitError" class="text-red-600 text-sm">{{ submitError }}</p>
        <div class="pt-4 flex items-center justify-between gap-3">
            <button
                type="button"
                @click="resetForm"
                class="border border-gray-300 text-gray-700 px-6 py-2 rounded hover:bg-gray-100 transition"
            >
                Űrlap törlése
            </button>
            <button
                type="submit"
                :disabled="submitting"
                class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 transition disabled:opacity-50"
            >
                {{ submitting ? 'Mentés...' : 'Mentés' }}
            </button>
        </div>
    </form>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { useRouter } from 'vue-router'
import { AllergenEnum, allergenList } from '@/types/recipe/allergens'
import type { Recipe, RecipeFormErrors } from '@/types/recipe/recipe'
import type { Ingredient } from '@/types/recipe/ingredient'
import { useAuthStore } from '@/stores/authStore'
import { normalizeIngredients, normalizeSteps, validateRecipeFields } from './recipeFormUtils'

const recipeStore = useRecipeStore()
const authStore = useAuthStore() // Assuming you have an auth store to get the authorId
const router = useRouter()

const imageFile = ref<File | null>(null)
const imageUrl = ref<string | null>(null)

function handleImageChange(e: Event) {
    if (!(e.target instanceof HTMLInputElement)) return
    const file = e.target.files?.[0]
    if (file) {
        imageFile.value = file
        imageUrl.value = URL.createObjectURL(file)
    }
}

const title = ref<string>('')
const description = ref<string>('')
const ingredients = ref<Ingredient[]>([
    { quantity: 0, unitOfMeasurement: '', name: '', allergens: [] },
])
const steps = ref<string[]>([''])
const selectedAllergens = ref<AllergenEnum[]>([])
const errors = ref<RecipeFormErrors>({})
const submitting = ref(false)
const submitError = ref<string | null>(null)

const allergenOptions = allergenList

function addIngredient() {
    ingredients.value.push({ quantity: 0, unitOfMeasurement: '', name: '', allergens: [] })
}
function removeIngredient(index: number) {
    ingredients.value.splice(index, 1)
}

function addStep() {
    steps.value.push('')
}
function removeStep(index: number) {
    steps.value.splice(index, 1)
}

function resetForm() {
    title.value = ''
    description.value = ''
    ingredients.value = [{ quantity: 0, unitOfMeasurement: '', name: '', allergens: [] }]
    steps.value = ['']
    selectedAllergens.value = []
    imageFile.value = null
    imageUrl.value = null
    errors.value = {}
    recipeStore.clearNewRecipeDraft(authStore.currentUser?.id)
}

function validateForm() {
    errors.value = {}
    if (!authStore.currentUser) {
        errors.value.title = 'Csak bejelentkezve lehet receptet feltölteni.'
        return false
    }
    const validation = validateRecipeFields(
        title.value,
        description.value,
        ingredients.value,
        steps.value,
    )

    errors.value = validation.errors

    return Object.keys(errors.value).length === 0
}

async function submit() {
    if (!validateForm() || submitting.value) return

    const normalizedIngredients = normalizeIngredients(ingredients.value)
    const normalizedSteps = normalizeSteps(steps.value)
    const newRecipe: Omit<Recipe, 'id'> = {
        authorId: authStore.getUserId,
        authorName: authStore.userName,
        title: title.value.trim(),
        description: description.value.trim(),
        ingredients: <Ingredient[]>(<unknown>normalizedIngredients), // TODO REMOVE THIS CAST
        steps: normalizedSteps,
        allergens: selectedAllergens.value,
        image: imageUrl.value || '',
        imageRevision: '',
        rating: 0,
    }

    submitting.value = true
    submitError.value = null

    try {
        const recipeId = await recipeStore.addRecipe(newRecipe)
        if (imageFile.value) {
            await recipeStore.updateImage(recipeId, imageFile.value)
        }
        router.push({ name: 'Recipe', params: { id: recipeId } })
        resetForm()
    } catch {
        submitError.value = 'Nem sikerült menteni a receptet. Próbáld újra!'
    } finally {
        submitting.value = false
    }
}

onMounted(() => {
    const draft = recipeStore.loadNewRecipeDraft(authStore.currentUser?.id)
    title.value = draft.title
    description.value = draft.description
    ingredients.value = draft.ingredients
    steps.value = draft.steps
    selectedAllergens.value = draft.selectedAllergens
})

watch(
    [title, description, ingredients, steps, selectedAllergens],
    () => {
        recipeStore.saveNewRecipeDraft(
            {
                title: title.value,
                description: description.value,
                ingredients: ingredients.value,
                steps: steps.value,
                selectedAllergens: selectedAllergens.value,
            },
            authStore.currentUser?.id,
        )
    },
    { deep: true },
)
</script>
