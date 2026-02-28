import type { CreateRecipeDTO, ReadRecipeDTO } from 'recipe-api-client'
import type { Recipe } from './recipe'
import { MapApiAllergenToEnum, MapEnumToApiAllergen } from './allergen.mappers'

export function MapApiRecipeToRecipe(apiRecipe: ReadRecipeDTO): Recipe {
    return {
        id: apiRecipe.id,
        authorId: 'unknown', // TODO get author info from API
        title: apiRecipe.title,
        description: apiRecipe.description ?? '',
        ingredients: apiRecipe.ingredients ?? [],
        steps: apiRecipe.steps ?? [],
        allergens: Array.from(apiRecipe.allergens ?? []).map((a) => MapApiAllergenToEnum(a)),
        image: 'empty', // TODO get image info from API, currently missing in API spec
        rating: 5, // TODO get rating info from API, currently missing in API spec
    }
}

export function MapRecipeToApiRecipe(recipe: Omit<Recipe, 'id'>): CreateRecipeDTO {
    return {
        title: recipe.title,
        description: recipe.description,
        steps: recipe.steps,
        ingredients: recipe.ingredients,
        allergens: new Set(recipe.allergens.map((a) => MapEnumToApiAllergen(a))),
    }
}
