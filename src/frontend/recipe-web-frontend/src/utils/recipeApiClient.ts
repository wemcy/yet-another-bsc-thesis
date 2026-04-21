import { Configuration, RecipeApiClient, IngredientsApi } from 'recipe-api-client'

const config = new Configuration({
    basePath: '/api',
    credentials: 'include',
})

export const recipeApiClient = new RecipeApiClient(config)

export const ingredientApiClient = new IngredientsApi(config)

export default {
    recipes: recipeApiClient,
    ingredients: ingredientApiClient,
}
