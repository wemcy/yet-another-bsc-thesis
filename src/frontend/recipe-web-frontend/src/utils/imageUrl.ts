import type { ImageSize } from 'recipe-api-client'

export function buildRecipeImageUrl(imagePath: string, imageRevision: string, size: ImageSize) {
    const query = new URLSearchParams({
        rev: imageRevision,
        size,
    })

    return `${imagePath}?${query.toString()}`
}
