import type { Recipe, RecipeState } from '@/types/recipe/recipe'
import { MapApiRecipeToRecipe, MapRecipeToApiRecipe } from '@/types/recipe/recipe.mappers'
import { defineStore } from 'pinia'
import { Configuration, RecipeApiClient } from 'recipe-api-client'

const api = new RecipeApiClient(
    new Configuration({
        // TODO make this dynamic based on env vars
        basePath: 'http://localhost:9393/api',
        credentials: 'include',
    }),
)
export const useRecipeStore = defineStore('recipe', {
    state: (): RecipeState => ({
        recipes: [],
        featuredRecipeId: null,
        showcaseRecipesIds: [],
        ownRecipeIds: [] as string[],
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
        featuredRecipe: (state) => state.recipes.find((r) => r.id === state.featuredRecipeId),
        showcaseRecipes: (state) =>
            state.recipes.filter((r) => state.showcaseRecipesIds.includes(r.id)),
        getCommentsByRecipeId: (state) => (id: string) => {
            return []
        },
        ownRecipes: (state) => state.recipes.filter((r) => state.ownRecipeIds.includes(r.id)),
    },

    actions: {
        async addRecipe(recipe: Omit<Recipe, 'id'>) {
            const response = await api.createRecipe({
                createRecipeDTO: MapRecipeToApiRecipe(recipe),
            })
            return response.id
        },
        async updateRating(id: string, rating: number) {
            const recipe = this.recipes.find((r) => r.id === id)
            const previousRating = recipe?.rating

            if (recipe) recipe.rating = rating

            try {
                await api.rateRecipe({
                    id,
                    rateRecipeRequest: { rating },
                })
            } catch (error) {
                if (recipe && previousRating !== undefined) {
                    recipe.rating = previousRating
                }
                throw error
            }

            await this.fetchRecipeById(id)
        },
        updateRecipe(recipe: Recipe) {
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
            else this.recipes.push(recipe)
        },

        async refreshRecipes() {
            await api.listRecipesPaginated({ page: 0, pageSize: 100 }).then((response) => {
                response.responseBody.forEach((apiRecipe) => {
                    const recipe = MapApiRecipeToRecipe(apiRecipe)
                    this.updateRecipe(recipe)
                })
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
        async fetchOwnRecipes(authorId: string) {
            const response = await api.getRecipesByAuthorId({ id: authorId })
            const mapped = response.map((r) => MapApiRecipeToRecipe(r))
            mapped.forEach((r) => this.updateRecipe(r))
            this.ownRecipeIds = mapped.map((r) => r.id)
        },
    },
})
