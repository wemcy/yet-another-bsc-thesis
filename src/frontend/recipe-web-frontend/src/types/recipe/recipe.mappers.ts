import type { CreateRecipeRequest, Recipe as RecipeDTO } from 'recipe-api-client'
import type { Recipe } from './recipe'
import { MapApiAllergenToEnum, MapEnumToApiAllergen } from './allergen.mappers'
import { MapApiIngredientToIngredient, MapIngredientToApiIngredient } from './ingredient.mappers'

export function MapApiRecipeToRecipe(apiRecipe: RecipeDTO): Recipe {
    const imageRevision = (apiRecipe.updatedAt ?? apiRecipe.createdAt).toISOString()

    return {
        id: apiRecipe.id,
        authorId: apiRecipe.creatorAuthorId ?? '',
        authorName: apiRecipe.creatorDisplayName ?? '',
        title: apiRecipe.title,
        description: apiRecipe.description ?? '',
        ingredients: Array.from(apiRecipe.ingredients ?? []).map(MapApiIngredientToIngredient),
        steps: apiRecipe.steps ?? [],
        allergens: Array.from(apiRecipe.allergens ?? []).map((a) => MapApiAllergenToEnum(a)),
        image: `/api/recipes/${apiRecipe.id}/image`,
        imageRevision,
        rating: apiRecipe.averageRating,
    }
}

export function MapRecipeToApiRecipe(recipe: Omit<Recipe, 'id'>): CreateRecipeRequest {
    return {
        title: recipe.title,
        description: recipe.description,
        steps: recipe.steps,
        ingredients: Array.from(recipe.ingredients ?? []).map(MapIngredientToApiIngredient),
        allergens: new Set(recipe.allergens.map((a) => MapEnumToApiAllergen(a))),
    }
}
