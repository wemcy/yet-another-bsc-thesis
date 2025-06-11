<template>
    <form @submit.prevent="submit" class="space-y-6">
        <!-- Title -->
        <div>
            <label class="block font-semibold mb-1">Recept neve</label>
            <input
                v-model="title"
                type="text"
                required
                class="w-full border rounded px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </div>

        <!-- Description -->
        <div>
            <label class="block font-semibold mb-1">Leírás</label>
            <textarea
                v-model="description"
                required
                class="w-full border rounded px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
                rows="3"
            ></textarea>
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
import { ref } from 'vue'

const title = ref('')
const description = ref('')
const ingredients = ref([{ amount: '', unit: '', name: '' }])
const steps = ref([''])
const selectedAllergens = ref<string[]>([])

const allergenOptions = [
    'Glutén',
    'Tej',
    'Tojás',
    'Hal',
    'Földimogyoró',
    'Szójabab',
    'Diófélék',
    'Zeller',
    'Mustár',
    'Szezámmag',
    'Kén-dioxid',
    'Csillagfürt',
    'Puhatestűek',
]

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

function submit() {
    console.log({
        title: title.value,
        description: description.value,
        ingredients: ingredients.value,
        steps: steps.value,
        allergens: selectedAllergens.value,
    })
}
</script>
