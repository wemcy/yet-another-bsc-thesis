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
        featuredRecipeId: null,
        showcaseRecipesIds: [],
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
        featuredRecipe: (state) => state.recipes.find((r) => r.id === state.featuredRecipeId),
        showcaseRecipes: (state) =>
            state.recipes.filter((r) => state.showcaseRecipesIds.includes(r.id)),
        getCommentsByRecipeId: (state) => (id: string) => {
            const recipe = state.recipes.find((r) => r.id === id)
            return recipe ? recipe.comments : []
        },
    },

    actions: {
        async addRecipe(recipe: Omit<Recipe, 'id'>) {
            const response = await api.createRecipe({
                createRecipeDTO: MapRecipeToApiRecipe(recipe),
            })
            return response.id
        },
        updateRating(id: string, rating: number) {
            const recipe = this.recipes.find((r) => r.id === id)
            if (recipe) recipe.rating = rating
        },
        updateRecipe(recipe: Recipe) {
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
            else this.recipes.push(recipe)
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
            this.updateRecipe(recipe)
        },
        async fetchShowcaseRecipes() {
            await api.listShowcaseRecipes().then((response) => {
                this.showcaseRecipesIds = response.map((r) => r.id)
                response.forEach((apiRecipe) => {
                    const recipe = MapApiRecipeToRecipe(apiRecipe)
                    this.updateRecipe(recipe)
                })
            })
        },
        async fetchFeaturedRecipe() {
            await api.getFeaturedRecipe().then((response) => {
                this.featuredRecipeId = response.id
                const recipe = MapApiRecipeToRecipe(response)
                this.updateRecipe(recipe)
            })
        },
        async updateImage(id: string, image: File) {
            await api.updateRecipeImage({ id, image })
        },
    },
})
