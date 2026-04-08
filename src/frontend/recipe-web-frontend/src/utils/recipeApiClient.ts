import { Configuration, RecipeApiClient } from 'recipe-api-client'

export const recipeApiClient = new RecipeApiClient(
    new Configuration({
        // TODO make this dynamic based on env vars
        basePath: 'http://localhost:9393/api',
        credentials: 'include',
    }),
)
