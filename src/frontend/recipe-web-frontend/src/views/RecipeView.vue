<template>
    <main class="max-w-6xl mx-auto px-4 py-10 text-gray-800 grid md:grid-cols-3 gap-10">
        <!-- Bal oszlop -->
        <div class="md:col-span-2 space-y-6">
            <div>
                <h1 class="text-3xl font-bold mb-2">Töltött paprika</h1>
                <p class="text-gray-700">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec pulvinar, purus
                    at rhoncus scelerisque...
                </p>
            </div>

            <div>
                <h2 class="text-xl font-semibold mb-2">Összetevők</h2>
                <ol class="list-decimal list-inside space-y-1">
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                </ol>
            </div>

            <div>
                <h2 class="text-xl font-semibold mb-2">Elkészítés</h2>
                <ol class="list-decimal list-inside space-y-1">
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                    <li>Lorem ipsum</li>
                </ol>
            </div>
        </div>

        <!-- Jobb oszlop -->
        <div class="space-y-6">
            <div class="flex items-center space-x-1 text-blue-500 text-xl">
                <span>★★★★☆</span>
                <!-- lehet később dinamikus értékelés -->
            </div>

            <img
                src="@/assets/recipe.jpg"
                alt="Recept kép"
                class="w-full h-64 object-cover rounded shadow"
            />

            <div>
                <h2 class="text-xl font-semibold mb-2">Allergének</h2>
                <ul class="list-disc list-inside text-sm text-gray-700">
                    <li>Glutén</li>
                    <li>Tej</li>
                </ul>
            </div>
        </div>
    </main>
    <!-- Komment szekció -->
    <section class="max-w-6xl mx-auto px-4 py-10 mt-10 border-t pt-6">
        <h2 class="text-xl font-semibold mb-4">Hozzászólások</h2>

        <!-- Egy komment -->
        <div class="space-y-6">
            <div class="border-b pb-4">
                <p class="font-semibold">Kiss Péter</p>
                <p class="text-gray-700">
                    Nagyon jó recept, múlt héten is elkészítettem, mindenki imádta!
                </p>
                <p class="text-sm text-gray-400 mt-1">2024. 11. 08.</p>
            </div>

            <div class="border-b pb-4">
                <p class="font-semibold">Nagy Eszter</p>
                <p class="text-gray-700">Kicsit több sóval még finomabb lett 😄</p>
                <p class="text-sm text-gray-400 mt-1">2024. 12. 01.</p>
            </div>
        </div>

        <!-- Új hozzászólás -->
        <form class="mt-6 space-y-4">
            <textarea
                rows="4"
                placeholder="Írd meg a véleményed..."
                class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            ></textarea>
            <button
                type="submit"
                class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 transition"
            >
                Hozzászólás küldése
            </button>
        </form>
    </section>
</template>
