<template>
    <nav
        class="bg-white shadow-md px-6 py-4 flex flex-col md:flex-row items-center md:justify-between gap-4 md:gap-0"
    >
        <!-- Logó -->
        <div class="text-2xl font-bold text-gray-800">ReceptApp</div>

        <!-- Kereső mező -->
        <div class="w-full md:w-1/2">
            <input
                type="text"
                placeholder="Keresés..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
        </div>

        <!-- Navigáció -->
        <ul class="flex space-x-6 text-gray-700">
            <li><router-link to="/" class="hover:text-blue-600">Receptek</router-link></li>
            <li>
                <router-link to="/uj-recept" class="hover:text-blue-600">Új recept</router-link>
            </li>
            <li><router-link to="/profil" class="hover:text-blue-600">Profil</router-link></li>
        </ul>
    </nav>
</template>
