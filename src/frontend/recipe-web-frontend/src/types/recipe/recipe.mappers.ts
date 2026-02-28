import type { ReadRecipeDTO } from 'recipe-api-client'
import type { Recipe } from './recipe'
import { MapApiAllergenToEnum } from './allergen.mappers'

export function MapApiRecipeToRecipe(apiRecipe: ReadRecipeDTO): Recipe {
    return {
        id: apiRecipe.id,
        authorId: 'unknown', // TODO get author info from API
        title: apiRecipe.title,
        description: apiRecipe.description ?? '',
        ingredients: [], // TODO map ingredients from API, currently missing in API spec
        steps: [], // TODO map steps from API, currently missing in API spec
        allergens: Array.from(apiRecipe.allergens ?? []).map((a) => MapApiAllergenToEnum(a)),
        image: 'empty', // TODO get image info from API, currently missing in API spec
        rating: 5, // TODO get rating info from API, currently missing in API spec
    }
}
