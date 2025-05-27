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
            class="max-w-6xl mx-auto flex lg:flex-cols items-start md:flex-row justify-between gap-6 text-center md:text-left"
        >
            <div class="flex flex-col items-center space-x-2 justify-center md:justify-start">
                <img src="../assets/logo.svg" alt="logo" class="w-24 h-24" />
                <span class="text-lg font-semibold">ReceptApp</span>
            </div>
            <nav class="flex flex-col items-start space-x-2 justify-center md:justify-start">
                <p class="text-lg font-bold just">Account</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Create account</a></li>
                    <li><a class="mb-4" href="#">Sign in</a></li>
                    <li><a class="mb-4" href="#">iOS app</a></li>
                    <li><a class="mb-4" href="#">Android app</a></li>
                </ul>
            </nav>
            <nav class="flex flex-col items-start space-x-2 justify-center md:justify-start">
                <p class="text-lg font-bold just">Company</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">About RecipeApp</a></li>
                    <li><a class="mb-4" href="#">For Business</a></li>
                    <li><a class="mb-4" href="#">Cooking partners</a></li>
                    <li><a class="mb-4" href="#">Careers</a></li>
                </ul>
            </nav>

            <nav class="flex flex-col items-start space-x-2 justify-center md:justify-start">
                <p class="text-lg font-bold just">Resources</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Recipe directory </a></li>
                    <li><a class="mb-4" href="#">Help center</a></li>
                    <li><a class="mb-4" href="#">Privacy & terms</a></li>
                </ul>
            </nav>

            <!-- Kapcsolat -->
            <div class="text-m self-center">
                <span>Elérhetőség: </span>
                <a href="mailto:info@receptapp.hu" class="hover:text-blue-600 block md:inline"
                    >info@receptapp.hu</a
                >
            </div>
        </div>
    </footer>
</template>
