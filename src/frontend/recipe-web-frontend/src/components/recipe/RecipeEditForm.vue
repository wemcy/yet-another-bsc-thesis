<template>
    <form @submit.prevent="submit" class="space-y-6">
        <!-- Title -->
        <div>
            <label class="block font-semibold mb-1">Recept neve</label>
            <input
                v-model="title"
                type="text"
                class="w-full border rounded px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
            <p v-if="errors.title" class="text-red-600 text-sm mt-1">{{ errors.title }}</p>
        </div>

        <!-- Description -->
        <div>
            <label class="block font-semibold mb-1">Leírás</label>
            <textarea
                v-model="description"
                class="w-full border rounded px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
                rows="3"
            ></textarea>
            <p v-if="errors.description" class="text-red-600 text-sm mt-1">
                {{ errors.description }}
            </p>
        </div>
        <div>
            <label class="block font-semibold mb-2">Értékelés</label>
            <div class="text-lg">{{ props.recipe.rating }} ⭐</div>
        </div>

        <!-- Ingredients -->
        <div>
            <label class="block font-semibold mb-2">Hozzávalók</label>
            <div v-for="(ingredient, index) in ingredients" :key="index" class="flex gap-2 mb-2">
                <input
                    v-model="ingredient.amount"
                    type="number"
                    placeholder="Mennyiség"
                    class="w-1/4 border rounded px-2 py-1"
                />
                <input
                    v-model="ingredient.unit"
                    type="text"
                    placeholder="Egység"
                    class="w-1/4 border rounded px-2 py-1"
                />
                <input
                    v-model="ingredient.name"
                    type="text"
                    placeholder="Hozzávaló"
                    class="w-full border rounded px-2 py-1"
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
                    class="w-full border rounded px-2 py-1"
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
                        class="accent-blue-600"
                    />
                    {{ item }}
                </label>
            </div>
        </div>

        <!-- Image upload -->
        <div>
            <label class="block font-semibold mb-2">Kép feltöltése</label>
            <input type="file" accept="image/*" @change="handleImageChange" />
            <div v-if="imageUrl" class="mt-2">
                <img :src="imageUrl" alt="Preview" class="w-64 h-40 object-cover rounded shadow" />
            </div>
        </div>

        <!-- Submit -->
        <div class="text-right pt-4">
            <button
                type="submit"
                class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 transition"
            >
                Mentés
            </button>
        </div>
    </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRecipeStore } from '@/stores/recipeStore'
import { useRouter } from 'vue-router'
import { allergenList } from '@/types/recipe/allergens.d'
import type { Recipe } from '@/types/recipe/recipe'

const props = defineProps<{ recipe: Recipe }>()
const recipeStore = useRecipeStore()
const router = useRouter()

const imageFile = ref<File | null>(null)
const imageUrl = ref<string | null>(null)

const title = ref('')
const description = ref('')
const ingredients = ref([{ amount: '', unit: '', name: '' }])
const steps = ref([''])
const selectedAllergens = ref<string[]>([])
const errors = ref<Record<string, string>>({})

const allergenOptions = allergenList

watch(
    () => props.recipe,
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
    ingredients.value.push({ amount: '', unit: '', name: '' })
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
    errors.value = {}
    if (!title.value.trim()) errors.value.title = 'A cím megadása kötelező.'
    if (!description.value.trim()) errors.value.description = 'A leírás nem lehet üres.'
    if (!ingredients.value.some((i) => i.name.trim()))
        errors.value.ingredients = 'Adj meg legalább egy hozzávalót.'
    if (!steps.value.some((s) => s.trim()))
        errors.value.steps = 'Legalább egy lépést meg kell adni.'

    return Object.keys(errors.value).length === 0
}

function submit() {
    if (!validateForm()) return

    const updatedRecipe = {
        ...props.recipe,
        title: title.value,
        description: description.value,
        ingredients: ingredients.value,
        steps: steps.value,
        allergens: selectedAllergens.value,
        image: imageUrl.value || props.recipe.image,
        // rating marad, ne változtasd!
    }

    recipeStore.updateRecipe(updatedRecipe)
    router.push({ name: 'Recipe', params: { id: props.recipe.id } })
}
</script>
