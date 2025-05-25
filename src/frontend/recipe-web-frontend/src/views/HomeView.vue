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
    <footer></footer>
</template>
