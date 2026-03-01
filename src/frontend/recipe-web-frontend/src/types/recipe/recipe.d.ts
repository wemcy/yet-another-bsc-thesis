import type { AllergenEnum } from './allergens'
import type { Ingredient } from './ingredient'

export type Recipe = {
    id: string
    authorId: string
    title: string
    description: string
    ingredients: Ingredient[]
    steps: string[]
    allergens: AllergenEnum[]
    image: string
    rating: number
}
export interface RecipeFormErrors {
    title?: string
    description?: string
    ingredients?: string
    steps?: string
}

export interface RecipeState {
    recipes: Recipe[]
    showcaseRecipesIds: Recipe.id[]
    featuredRecipeId: Recipe.id | null
}
