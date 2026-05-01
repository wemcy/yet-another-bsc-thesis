import type { ImageSize } from 'recipe-api-client'

export function buildRecipeImageUrl(imagePath: string, size: ImageSize) {
    const query = new URLSearchParams({
        size,
    })

    return `${imagePath}?${query.toString()}`
}
