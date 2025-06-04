<script setup lang="ts"></script>

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
    <nav>
        <h1>Recipes</h1>
        <input type="search" />
        <button>Send in</button>
        <button>Profile</button>
    </nav>
    <main>Hello World!</main>

    <footer class="bg-gray-100 text-gray-700 py-6 px-4 mt-10">
        <div
            class="max-w-6xl mx-auto flex flex-center sm:items-start sm:flex-row flex-col justify-between gap-6 sm:flex-left flex-center"
        >
            <div
                role="banner"
                class="flex flex-col items-center space-y-2 justify-center sm:justify-start"
            >
                <img src="../assets/logo.svg" alt="logo" class="w-24 h-24" />
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
            <div class="text-base self-center">
                <span>Connect: </span>
                <a href="mailto:info@receptapp.hu" class="hover:text-blue-600 block md:inline"
                    >info@receptapp.hu</a
                >
            </div>
        </div>
    </footer>
</template>
