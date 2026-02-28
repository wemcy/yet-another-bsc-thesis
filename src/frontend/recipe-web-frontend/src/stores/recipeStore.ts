import type { Recipe, RecipeState } from '@/types/recipe/recipe'
import { MapApiRecipeToRecipe, MapRecipeToApiRecipe } from '@/types/recipe/recipe.mappers'
import { defineStore } from 'pinia'
import { Configuration, RecipesApi } from 'recipe-api-client'

const api = new RecipesApi(
    new Configuration({
        // TODO make this dynamic based on env vars
        basePath: 'http://localhost:9393/api',
    }),
)
export const useRecipeStore = defineStore('recipe', {
    state: (): RecipeState => ({
        recipes: [],
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
        featuredRecipe: (state) => state.recipes[0] || null,
    },

    actions: {
        async addRecipe(recipe: Omit<Recipe, 'id'>) {
            await api.createRecipe({ createRecipeDTO: MapRecipeToApiRecipe(recipe) })
        },
        updateRating(id: string, rating: number) {
            const recipe = this.recipes.find((r) => r.id === id)
            if (recipe) recipe.rating = rating
        },
        updateRecipe(recipe: Recipe) {
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
        },
        async refreshRecipes() {
            await api.listRecipes().then((response) => {
                this.recipes = response.map((apiRecipe) => MapApiRecipeToRecipe(apiRecipe))
            })
        },
        async fetchRecipeById(id: string) {
            const recipe = await api
                .getRecipeById({ id })
                .then((apiRecipe) => MapApiRecipeToRecipe(apiRecipe))
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
            else this.recipes.push(recipe)
        },
    },
})
