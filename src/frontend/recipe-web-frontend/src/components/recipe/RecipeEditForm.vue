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
        <div>
            <label class="block font-semibold mb-2">Értékelés</label>
            <div class="text-lg">{{ recipe.rating }} ⭐</div>
        </div>

        <!-- Ingredients -->
        <div>
            <label class="block font-semibold mb-2">Hozzávalók</label>
            <div v-for="(ingredient, index) in ingredients" :key="index" class="flex gap-2 mb-2">
                <input
                    v-model.number="ingredient.quantity"
                    type="number"
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

        <!-- Submit -->
        <p v-if="submitError" class="text-red-600 text-sm">{{ submitError }}</p>
        <div class="pt-4 flex items-center justify-between gap-3">
            <button
                type="button"
                @click="router.push({ name: 'Recipe', params: { id: recipe.id } })"
                class="border border-gray-300 text-gray-700 px-6 py-2 rounded hover:bg-gray-100 transition"
            >
                Mégse
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
import { ref, watch } from 'vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { useRouter } from 'vue-router'
import { AllergenEnum, allergenList } from '@/types/recipe/allergens'
import type { Recipe, RecipeFormErrors } from '@/types/recipe/recipe'
import type { Ingredient } from '@/types/recipe/ingredient'
import { normalizeIngredients, normalizeSteps, validateRecipeFields } from './recipeFormUtils'
const { recipe } = defineProps<{ recipe: Recipe }>()
const recipeStore = useRecipeStore()
const router = useRouter()

const title = ref<string>('')
const description = ref<string>('')
const ingredients = ref<Ingredient[]>([{ quantity: 0, unitOfMeasurement: '', name: '' }])
const imageFile = ref<File | null>(null)
const imageUrl = ref<string | null>(null)

const steps = ref<string[]>([''])
const selectedAllergens = ref<AllergenEnum[]>([])
const errors = ref<RecipeFormErrors>({})

const allergenOptions = allergenList

watch(
    () => recipe,
    (newRecipe: Recipe) => {
        if (newRecipe) {
            title.value = newRecipe.title
            description.value = newRecipe.description
            ingredients.value = [...newRecipe.ingredients]
            steps.value = [...newRecipe.steps]
            selectedAllergens.value = [...newRecipe.allergens]
            imageUrl.value = newRecipe.image || null
        }
    },
    { immediate: true },
)

function handleImageChange(e: Event) {
    const file = (e.target as HTMLInputElement)?.files?.[0]
    if (file) {
        imageFile.value = file
        imageUrl.value = URL.createObjectURL(file)
    }
}

function addIngredient() {
    ingredients.value.push({ quantity: 0, unitOfMeasurement: '', name: '' })
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

function validateForm() {
    const validation = validateRecipeFields(
        title.value,
        description.value,
        ingredients.value,
        steps.value,
    )

    errors.value = validation.errors

    return Object.keys(errors.value).length === 0
}

const submitting = ref(false)
const submitError = ref<string | null>(null)

async function submit() {
    if (!validateForm() || submitting.value) return

    submitting.value = true
    submitError.value = null

    try {
        const normalizedIngredients = normalizeIngredients(ingredients.value)
        const normalizedSteps = normalizeSteps(steps.value)

        await recipeStore.updateRecipeById(
            recipe.id,
            {
                title: title.value.trim(),
                description: description.value.trim(),
                ingredients: normalizedIngredients,
                steps: normalizedSteps,
                allergens: selectedAllergens.value,
                image: recipe.image,
                authorId: recipe.authorId,
                authorName: recipe.authorName,
                rating: recipe.rating,
            },
            imageFile.value,
        )
        router.push({ name: 'Recipe', params: { id: recipe.id } })
    } catch {
        submitError.value = 'Nem sikerült menteni a receptet. Próbáld újra!'
    } finally {
        submitting.value = false
    }
}
</script>
