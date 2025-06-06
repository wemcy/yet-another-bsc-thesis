<script setup lang="ts">
import ProfileEdit from './ProfileView.vue'
import RecipeView from './RecipeView.vue'
</script>

<template>
    <nav class="bg-white shadow-md px-6 py-4 flex justify-between items-center">
        <div class="text-2xl font-bold text-gray-800">ReceptApp</div>
        <div class="flex-1 mx-6">
            <input
                type="text"
                placeholder="Keresés..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </div>
        <ul class="flex flex-2 justify-end space-x-6 text-gray-700">
            <li><a href="#" class="hover:text-blue-600">Receptek</a></li>
            <li><a href="#" class="hover:text-blue-600">Új recept</a></li>
            <li><a href="#" class="hover:text-blue-600">Profil</a></li>
        </ul>
    </nav>
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

        <section class="max-w-6xl mx-auto px-4 py-10">
            <div class="bg-white shadow-lg rounded-lg overflow-hidden flex flex-col md:flex-row">
                <img
                    src="@/assets/recipe.jpg"
                    alt="Kiemelt recept"
                    class="w-full md:w-1/2 h-64 object-cover"
                />
                <div class="p-6 flex flex-col justify-center">
                    <h2 class="text-2xl font-bold mb-2">Friss recept: Töltött paprika</h2>
                    <p class="text-gray-600 mb-4">
                        Egy klasszikus magyar kedvenc, ahogy nagymamánk készítette. Egyszerű,
                        laktató, isteni!
                    </p>
                    <button
                        class="self-start bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
                    >
                        Megnézem
                    </button>
                </div>
            </div>
        </section>
        <section class="max-w-6xl mx-auto px-4 py-12">
            <h2 class="text-2xl font-bold mb-6 text-center">Kiemelt receptek</h2>
            <div class="grid gap-6 md:grid-cols-3">
                <!-- Receptkártya 1 -->
                <div class="bg-white shadow-md rounded-lg overflow-hidden">
                    <img
                        src="@/assets/recipe.jpg"
                        alt="Paprikás krumpli"
                        class="w-full h-40 object-cover"
                    />
                    <div class="p-4">
                        <h3 class="font-semibold text-lg mb-2">Paprikás krumpli</h3>
                        <p class="text-sm text-gray-600">Allergén: gluténmentes</p>
                    </div>
                </div>

                <!-- Receptkártya 2 -->
                <div class="bg-white shadow-md rounded-lg overflow-hidden">
                    <img
                        src="@/assets/recipe.jpg"
                        alt="Rakott karfiol"
                        class="w-full h-40 object-cover"
                    />
                    <div class="p-4">
                        <h3 class="font-semibold text-lg mb-2">Rakott karfiol</h3>
                        <p class="text-sm text-gray-600">Allergén: tej</p>
                    </div>
                </div>

                <!-- Receptkártya 3 -->
                <div class="bg-white shadow-md rounded-lg overflow-hidden">
                    <img
                        src="@/assets/recipe.jpg"
                        alt="Tojásos nokedli"
                        class="w-full h-40 object-cover"
                    />
                    <div class="p-4">
                        <h3 class="font-semibold text-lg mb-2">Tojásos nokedli</h3>
                        <p class="text-sm text-gray-600">Allergén: glutén, tojás</p>
                    </div>
                </div>
            </div>
        </section>
    </main>

    <RecipeView></RecipeView>
    <main class="max-w-4xl mx-auto px-4 py-10 text-gray-800">
        <!-- Profil fejléc -->
        <section class="flex flex-col sm:flex-row items-center sm:items-start gap-6 mb-10">
            <img
                src="@/assets/profile.svg"
                alt="Profilkép"
                class="w-32 h-32 rounded-full object-cover"
            />
            <div class="text-center sm:text-left">
                <h1 class="text-2xl font-bold">Kovács Anna</h1>
                <p class="text-gray-600">anna.kovacs@example.com</p>
            </div>
        </section>
    </main>
    <ProfileEdit> </ProfileEdit>
    <footer class="bg-gray-100 text-gray-700 py-6 px-4 mt-10">
        <div
            class="max-w-6xl mx-auto flex sm:items-start sm:flex-row flex-col justify-between gap-6"
        >
            <div
                role="banner"
                class="flex flex-col items-center space-y-2 justify-center sm:justify-start"
            >
                <img src="../assets/logo.svg" alt="ReceptApp logó" class="w-24 h-24" />
                <span class="text-lg font-semibold">ReceptApp</span>
            </div>
            <nav aria-label="Account" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold just">Account</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Create account</a></li>
                    <li><a class="mb-4" href="#">Sign in</a></li>
                    <li><a class="mb-4" href="#">iOS app</a></li>
                    <li><a class="mb-4" href="#">Android app</a></li>
                </ul>
            </nav>
            <nav aria-label="Company" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold just">Company</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">About RecipeApp</a></li>
                    <li><a class="mb-4" href="#">For Business</a></li>
                    <li><a class="mb-4" href="#">Cooking partners</a></li>
                    <li><a class="mb-4" href="#">Careers</a></li>
                </ul>
            </nav>

            <nav aria-label="Resources" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold just">Resources</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Recipe directory </a></li>
                    <li><a class="mb-4" href="#">Help center</a></li>
                    <li><a class="mb-4" href="#">Privacy & terms</a></li>
                </ul>
            </nav>

            <!-- Kapcsolat -->
            <div class="text-base self-center flex flex-col sm:items-start items-center">
                <span>Connect: </span>
                <a href="mailto:info@receptapp.hu" class="hover:text-blue-600 block md:inline"
                    >info@receptapp.hu</a
                >
            </div>
        </div>
    </footer>
</template>
