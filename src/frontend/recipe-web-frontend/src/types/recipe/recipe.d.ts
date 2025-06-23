import type { Allergen } from './allergens'

export type Recipe = {
    id: string
    authorId: string
    title: string
    description: string
    ingredients: Ingredient[]
    steps: string[]
    allergens: Allergen[]
    image: string
    rating: number
}
interface RecipeFormErrors {
    title?: string
    description?: string
    ingredients?: string
    steps?: string
}
