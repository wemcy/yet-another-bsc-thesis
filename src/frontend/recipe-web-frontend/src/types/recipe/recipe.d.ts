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
    comments: Comment[]
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

export interface Comment {
    id: string
    authorId: string
    content: string
    createdAt: Date
}

export interface CommentPayload {
    authorId: string
    content: string
}
