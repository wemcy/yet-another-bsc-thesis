import { defineStore } from 'pinia'

export const useRecipeStore = defineStore('recipe', {
    state: () => ({
        recipes: [
            {
                id: '1',
                title: 'Töltött paprika',
                description: 'Egy klasszikus magyar kedvenc, ahogy nagymamánk készítette.',
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4,
                allergens: ['Glutén', 'Tej'],
                ingredients: [
                    { name: 'Paprika', amount: 4, unit: 'db' },
                    { name: 'Darálthús', amount: 500, unit: 'g' },
                    { name: 'Rizs', amount: 100, unit: 'g' },
                ],
                steps: [
                    'A rizst előfőzzük enyhén sós vízben.',
                    'A darált húst fűszerezzük, összekeverjük a rizzsel.',
                    'A paprikákat megtöltjük, majd paradicsomszószban megfőzzük.',
                ],
            },
            // Ide jöhetnek további receptek
        ],
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
    },
})
