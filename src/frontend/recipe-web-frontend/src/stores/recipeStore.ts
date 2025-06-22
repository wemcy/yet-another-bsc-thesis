import type { Recipe } from '@/types/recipe/recipe'
import { defineStore } from 'pinia'

export const useRecipeStore = defineStore('recipe', {
    state: () => ({
        recipes: [
            {
                id: '1',
                authorId: '1',
                title: 'Töltött paprika',
                description: 'Egy klasszikus magyar kedvenc darált hússal és paradicsomszósszal.',
                ingredients: [
                    { name: 'paprika', amount: 6, unit: 'db' },
                    { name: 'darált hús', amount: 500, unit: 'g' },
                    { name: 'rizs', amount: 100, unit: 'g' },
                ],
                steps: [
                    'Paprikát megmosni, kicsumázni',
                    'Húst összekeverni rizzsel és fűszerekkel',
                    'Tölteni, főzni paradicsomszószban',
                ],
                allergens: ['Glutén'],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.7,
            },
            {
                id: '2',
                authorId: '1',
                title: 'Paprikás krumpli',
                description: 'Egy gyors, egyszerű és laktató magyar étel kolbásszal.',
                ingredients: [
                    { name: 'burgonya', amount: 1, unit: 'kg' },
                    { name: 'kolbász', amount: 200, unit: 'g' },
                    { name: 'vöröshagyma', amount: 1, unit: 'db' },
                ],
                steps: [
                    'Hagymát dinsztelni',
                    'Fűszerezni',
                    'Krumplit, kolbászt hozzáadni, megfőzni',
                ],
                allergens: [],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.2,
            },
            {
                id: '3',
                authorId: '1',
                title: 'Rakott karfiol',
                description: 'Sült karfiolos egytálétel tejföllel és sajttal rétegezve.',
                ingredients: [
                    { name: 'karfiol', amount: 1, unit: 'db' },
                    { name: 'tejföl', amount: 200, unit: 'g' },
                    { name: 'sajt', amount: 100, unit: 'g' },
                ],
                steps: [
                    'Karfiolt főzni',
                    'Rétegezni rizzsel, tejföllel, sajttal',
                    'Sütni 30 percig',
                ],
                allergens: ['Tej'],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.6,
            },
            {
                id: '4',
                authorId: '1',
                title: 'Tojásos nokedli',
                description: 'Puha nokedli tojással összesütve, friss salátával tálalva.',
                ingredients: [
                    { name: 'liszt', amount: 300, unit: 'g' },
                    { name: 'tojás', amount: 3, unit: 'db' },
                    { name: 'víz', amount: 150, unit: 'ml' },
                ],
                steps: [
                    'Tésztát keverni',
                    'Szaggatni forró vízbe',
                    'Lecsöpögtetni, tojással összesütni',
                ],
                allergens: ['Glutén', 'Tojás'],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.5,
            },
            {
                id: '5',
                authorId: '1',
                title: 'Lecsó',
                description: 'Zöldséges magyar klasszikus, kenyérrel vagy tojással tálalva.',
                ingredients: [
                    { name: 'paradicsom', amount: 4, unit: 'db' },
                    { name: 'paprika', amount: 5, unit: 'db' },
                    { name: 'hagyma', amount: 1, unit: 'db' },
                ],
                steps: [
                    'Hagymát dinsztelni',
                    'Paprikát, paradicsomot hozzáadni',
                    'Főzni, fűszerezni',
                ],
                allergens: [],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.0,
            },
            {
                id: '6',
                authorId: '1',
                title: 'Tejfölös gombapaprikás',
                description: 'Selymes, tejfölös gombaragu, galuskával a legjobb.',
                ingredients: [
                    { name: 'gomba', amount: 500, unit: 'g' },
                    { name: 'tejföl', amount: 200, unit: 'g' },
                    { name: 'vöröshagyma', amount: 1, unit: 'db' },
                ],
                steps: ['Hagymát dinsztelni', 'Gombát pirítani', 'Tejföllel besűríteni'],
                allergens: ['Tej'],
                image: new URL('@/assets/recipe.jpg', import.meta.url).href,
                rating: 4.3,
            },
        ],
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
        featuredRecipe: (state) => state.recipes[0] || null,
    },

    actions: {
        addRecipe(recipe: Omit<Recipe, 'id'>) {
            const newRecipe = {
                id: Date.now().toString(),
                ...recipe,
            }
            this.recipes.push(newRecipe)
        },
        updateRating(id: string, rating: number) {
            const recipe = this.recipes.find((r) => r.id === id)
            if (recipe) recipe.rating = rating
        },
        updateRecipe(recipe: Recipe) {
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
        },
    },
})
