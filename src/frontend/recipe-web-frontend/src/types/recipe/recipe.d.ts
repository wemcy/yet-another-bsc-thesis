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

export interface NewRecipeDraft {
    title: string
    description: string
    ingredients: Ingredient[]
    steps: string[]
    selectedAllergens: AllergenEnum[]
}

export interface RecipeState {
    recipes: Recipe[]
    showcaseRecipesIds: Recipe.id[]
    featuredRecipeId: Recipe.id | null
    ownRecipeIds: Recipe.id[]
    newRecipeDraft: NewRecipeDraft
    showcaseRecipesLoading: boolean
    featuredRecipeLoading: boolean
    commentsByRecipeId: Record<string, Comment[]>
    commentsPaginationByRecipeId: Record<string, PaginationState>
    commentsLoadingByRecipeId: Record<string, boolean>
}

export interface PaginationState {
    pageNumber: number
    pageSize: number
    totalCount: number
    pageCount: number
    hasNextPage: boolean
    hasPreviousPage: boolean
}

export interface PaginatedRecipes {
    items: Recipe[]
    pagination: PaginationState
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

export interface PaginatedComments {
    items: Comment[]
    pagination: PaginationState
}
