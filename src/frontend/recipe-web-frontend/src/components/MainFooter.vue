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
