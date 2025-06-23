ChatGPT said: Persze, itt a magyarított verzió: vue Copy Edit
<template>
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
            <nav aria-label="Fiók" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold">Fiók</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Fiók létrehozása</a></li>
                    <li><a class="mb-4" href="#">Bejelentkezés</a></li>
                    <li><a class="mb-4" href="#">iOS alkalmazás</a></li>
                    <li><a class="mb-4" href="#">Android alkalmazás</a></li>
                </ul>
            </nav>
            <nav aria-label="Cég" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold">Cég</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">A ReceptApp-ról</a></li>
                    <li><a class="mb-4" href="#">Üzleti felhasználóknak</a></li>
                    <li><a class="mb-4" href="#">Főzőpartnerek</a></li>
                    <li><a class="mb-4" href="#">Karrier</a></li>
                </ul>
            </nav>
            <nav aria-label="Források" class="flex flex-col sm:items-start items-center">
                <p class="text-lg font-bold">Források</p>
                <ul class="mt-6">
                    <li><a class="mb-4" href="#">Recept katalógus</a></li>
                    <li><a class="mb-4" href="#">Súgóközpont</a></li>
                    <li><a class="mb-4" href="#">Adatvédelem és felhasználási feltételek</a></li>
                </ul>
            </nav>
            <!-- Kapcsolat -->
            <div class="text-base self-center flex flex-col sm:items-start items-center">
                <span>Kapcsolat:</span>
                <a href="mailto:info@receptapp.hu" class="hover:text-blue-600 block md:inline">
                    info@receptapp.hu
                </a>
            </div>
        </div>
    </footer>
</template>
