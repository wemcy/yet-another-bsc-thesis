import type { RecipeFormErrors } from '@/types/recipe/recipe'
import type { Ingredient } from '@/types/recipe/ingredient'

type IngredientDraft = Ingredient

type QuantityInput = number | string | null | undefined

const normalizeQuantity = (value: QuantityInput): number => {
    if (typeof value === 'number') return Number.isFinite(value) ? value : 0
    if (typeof value === 'string') {
        const normalized = Number(value.replace(',', '.'))
        return Number.isFinite(normalized) ? normalized : 0
    }
    return 0
}

const hasIngredientContent = (ingredient: IngredientDraft) =>
    ingredient.name.trim().length > 0 ||
    ingredient.unitOfMeasurement.trim().length > 0 ||
    normalizeQuantity(ingredient.quantity) > 0

export const normalizeIngredients = (ingredients: IngredientDraft[]) =>
    ingredients
        .map((ingredient) => ({
            name: ingredient.name.trim(),
            unitOfMeasurement: ingredient.unitOfMeasurement.trim(),
            quantity: normalizeQuantity(ingredient.quantity),
        }))
        .filter(hasIngredientContent)

export const normalizeSteps = (steps: string[]) =>
    steps.map((step) => step.trim()).filter((step) => step.length > 0)

export const validateRecipeFields = (
    title: string,
    description: string,
    ingredients: IngredientDraft[],
    steps: string[],
) => {
    const errors: RecipeFormErrors = {}
    const normalizedIngredients = normalizeIngredients(ingredients)
    const normalizedSteps = normalizeSteps(steps)

    if (!title.trim()) errors.title = 'A cím megadása kötelező.'
    if (!description.trim()) errors.description = 'A leírás nem lehet üres.'

    if (normalizedIngredients.length === 0) {
        errors.ingredients = 'Adj meg legalább egy hozzávalót.'
    } else if (
        normalizedIngredients.some(
            (ingredient) =>
                !ingredient.name || !ingredient.unitOfMeasurement || ingredient.quantity <= 0,
        )
    ) {
        errors.ingredients =
            'Minden megadott hozzávalónál töltsd ki a mennyiséget, az egységet és a hozzávaló nevét.'
    }

    if (normalizedSteps.length === 0) {
        errors.steps = 'Legalább egy lépést meg kell adni.'
    }

    return {
        errors,
        normalizedIngredients,
        normalizedSteps,
    }
}
