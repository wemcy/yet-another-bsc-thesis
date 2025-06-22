export type Recipe = {
    id: string
    authorId: string
    title: string
    description: string
    ingredients: Ingredient[]
    steps: string[]
    allergens: string[]
    image: string
    rating: number
}
