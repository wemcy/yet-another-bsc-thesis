import { MapApiAllergenToEnum, MapEnumToApiAllergen } from './allergen.mappers'
import type { Ingredient } from './ingredient'
import type { Ingredient as ApiIngredient } from 'recipe-api-client'

export function MapApiIngredientToIngredient(apiIngredient: ApiIngredient): Ingredient {
    return {
        name: apiIngredient.name,
        quantity: apiIngredient.quantity,
        unitOfMeasurement: apiIngredient.unitOfMeasurement,
        allergens: Array.from(apiIngredient.allergens ?? []).map((a) => MapApiAllergenToEnum(a)),
    }
}

export function MapIngredientToApiIngredient(ingredient: Ingredient): ApiIngredient {
    return {
        name: ingredient.name,
        quantity: ingredient.quantity,
        unitOfMeasurement: ingredient.unitOfMeasurement,
        allergens: new Set(ingredient.allergens.map((a) => MapEnumToApiAllergen(a))),
    }
}
