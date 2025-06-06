<script setup lang="ts">
import RecipeHighlight from '@/components/recipe/RecipeHighlight.vue'
import ProfileEdit from './ProfileView.vue'
import RecipeView from './RecipeView.vue'
import RecipeCard from '@/components/recipe/RecipeCard.vue'
</script>

<template>
    <main class="text-gray-800">
        <section class="text-center py-20 bg-white px-4">
            <h1 class="text-4xl font-bold mb-4">Fedezd fel a legjobb házias recepteket</h1>
            <p class="text-lg text-gray-600 mb-6">Egyszerűen kereshető, allergénszűrővel</p>
            <button
                class="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition"
            >
                Böngéssz receptek között
            </button>
        </section>

        <section class="max-w-4xl mx-auto px-4 py-10">
            <input
                type="text"
                placeholder="Keresés..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </section>
        <RecipeHighlight
            title="Friss recept: Töltött paprika"
            description="Egy klasszikus magyar kedvenc, ahogy nagymamánk készítette. Egyszerű, laktató, isteni!"
            image="@/assets/recipe.jpg"
        />
        <section class="max-w-6xl mx-auto px-4 py-12">
            <h2 class="text-2xl font-bold mb-6 text-center">Kiemelt receptek</h2>
            <div class="grid gap-6 md:grid-cols-3">
                <!-- Receptkártya 1 -->
                <RecipeCard
                    title="Paprikás krumpli"
                    image="@/assets/recipe.jpg"
                    allergens="gluténmentes"
                />

                <!-- Receptkártya 2 -->
                <RecipeCard
                    title="Paprikás krumpli"
                    image="@/assets/recipe.jpg"
                    allergens="gluténmentes"
                />

                <!-- Receptkártya 3 -->
                <RecipeCard
                    title="Paprikás krumpli"
                    image="@/assets/recipe.jpg"
                    allergens="gluténmentes"
                />
            </div>
        </section>
    </main>

    <RecipeView></RecipeView>
    <main class="max-w-4xl mx-auto px-4 py-10 text-gray-800">
        <!-- Profil fejléc -->
    </main>
    <ProfileEdit> </ProfileEdit>
</template>
