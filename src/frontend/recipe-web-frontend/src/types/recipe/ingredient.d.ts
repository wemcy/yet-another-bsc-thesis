import type { AllergenEnum } from '@/types/recipe/allergens'

export interface Ingredient {
    name: string
    quantity: number
    unitOfMeasurement: string
    allergens: AllergenEnum[]
}
