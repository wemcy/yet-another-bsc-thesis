<script setup lang="ts">
import CommentsSection from '@/components/comments/CommentsSection.vue'
import IngredientList from '@/components/recipe/IngredientList.vue'
import RecipeHeader from '@/components/recipe/RecipeHeader.vue'
import InstructionsList from '@/components/recipe/InstuctionList.vue'
import AllergenList from '@/components/recipe/AllergenList.vue'
import RecipeRating from '@/components/recipe/RecipeRating.vue'
import recipeImg from '@/assets/recipe.jpg'

const ingredients = [
    { name: 'Paprika', amount: 4, unit: 'db' },
    { name: 'Darálthús', amount: 500, unit: 'g' },
    { name: 'Rizs', amount: 100, unit: 'g' },
]

const steps = [
    'A rizst előfőzzük enyhén sós vízben.',
    'A darált húst fűszerezzük, összekeverjük a rizzsel.',
    'A paprikákat megtöltjük, majd paradicsomszószban megfőzzük.',
]

const allergens = ['Glutén', 'Tej']
const title = 'Töltött paprika'
const description =
    'Egy klasszikus magyar kedvenc, ahogy nagymamánk készítette. Egyszerű, laktató, isteni!'
const picture = { src: recipeImg, alt: 'Töltött paprika' }
const rating = 4
</script>

<template>
    <main class="max-w-6xl mx-auto px-4 py-10 text-gray-800 grid md:grid-cols-3 gap-10">
        <!-- Bal oszlop -->
        <div class="md:col-span-2 space-y-6">
            <RecipeHeader :title="title" :description="description" />
            <IngredientList :ingredients="ingredients" />
            <InstructionsList :steps="steps" />
        </div>

        <!-- Jobb oszlop -->
        <div class="space-y-6">
            <RecipeRating :rating="rating" />
            <img
                :src="picture.src"
                :alt="picture.alt"
                class="w-full h-64 object-cover rounded shadow"
            />
            <AllergenList :allergens="allergens" />
        </div>
    </main>
    <CommentsSection />
</template>
