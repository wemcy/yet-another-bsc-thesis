import { Configuration, RecipeApiClient } from 'recipe-api-client'

export const recipeApiClient = new RecipeApiClient(
    new Configuration({
        basePath: '/api',
        credentials: 'include',
    }),
)
