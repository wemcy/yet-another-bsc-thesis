import type { PaginatedRecipes, PaginationState, Recipe } from '@/types/recipe/recipe'
import { MapApiRecipeToRecipe } from '@/types/recipe/recipe.mappers'
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import { defineStore } from 'pinia'
import { useRecipeStore } from './recipeStore'
import type { Allergen } from 'recipe-api-client'

const createDefaultPaginationState = (pageNumber: number, pageSize: number): PaginationState => ({
    pageNumber,
    pageSize,
    totalCount: 0,
    pageCount: 0,
    hasNextPage: false,
    hasPreviousPage: pageNumber > 0,
})

const parseBoolean = (value: unknown): boolean | undefined => {
    if (typeof value === 'boolean') return value
    return undefined
}

const parseNumber = (value: unknown): number | undefined => {
    if (typeof value === 'number' && Number.isFinite(value)) return value
    return undefined
}

const parsePaginationState = (
    headerValue: string | null,
    fallbackPageNumber: number,
    fallbackPageSize: number,
): PaginationState => {
    if (!headerValue) {
        return createDefaultPaginationState(fallbackPageNumber, fallbackPageSize)
    }

    try {
        const raw = JSON.parse(headerValue) as Record<string, unknown>
        const pageNumber =
            parseNumber(raw.pageNumber) ?? parseNumber(raw.PageNumber) ?? fallbackPageNumber
        const pageSize = parseNumber(raw.pageSize) ?? parseNumber(raw.PageSize) ?? fallbackPageSize
        const totalCount = parseNumber(raw.totalCount) ?? parseNumber(raw.TotalCount) ?? 0
        const pageCount = parseNumber(raw.pageCount) ?? parseNumber(raw.PageCount) ?? 0
        const hasNextPage = parseBoolean(raw.hasNextPage) ?? parseBoolean(raw.HasNextPage) ?? false
        const hasPreviousPage =
            parseBoolean(raw.hasPreviousPage) ?? parseBoolean(raw.HasPreviousPage) ?? pageNumber > 0

        return {
            pageNumber,
            pageSize,
            totalCount,
            pageCount,
            hasNextPage,
            hasPreviousPage,
        }
    } catch {
        return createDefaultPaginationState(fallbackPageNumber, fallbackPageSize)
    }
}

export const useRecipePaginationStore = defineStore('recipePagination', {
    state: () => ({
        allRecipes: [] as Recipe[],
        allRecipesPagination: createDefaultPaginationState(0, 27),
        allRecipesLoading: false,
        ownRecipes: [] as Recipe[],
        ownRecipesPagination: createDefaultPaginationState(0, 27),
        ownRecipesLoading: false,
    }),
    actions: {
        async loadAllRecipesPage(
            pageNumber: number,
            pageSize: number,
            includeAllergens: Allergen[] = [],
            excludeAllergens: Allergen[] = [],
        ): Promise<PaginatedRecipes> {
            this.allRecipesLoading = true
            try {
                const response = await api.listRecipesRaw({
                    page: pageNumber,
                    pageSize,
                    includeAllergens:
                        includeAllergens.length > 0 ? new Set(includeAllergens) : null,
                    excludeAllergens:
                        excludeAllergens.length > 0 ? new Set(excludeAllergens) : null,
                })
                const body = await response.value()
                const items = body.map((r) => MapApiRecipeToRecipe(r))
                const pagination = parsePaginationState(
                    response.raw.headers.get('X-Pagination'),
                    pageNumber,
                    pageSize,
                )

                this.allRecipes = items
                this.allRecipesPagination = pagination

                const recipeStore = useRecipeStore()
                items.forEach((item) => recipeStore.updateRecipe(item))

                return { items, pagination }
            } finally {
                this.allRecipesLoading = false
            }
        },
        async loadOwnRecipesPage(
            authorId: string,
            pageNumber: number,
            pageSize: number,
        ): Promise<PaginatedRecipes> {
            this.ownRecipesLoading = true
            try {
                const response = await api.getRecipesByAuthorIdRaw({
                    id: authorId,
                    page: pageNumber,
                    pageSize,
                })
                const body = await response.value()
                const items = body.map((r) => MapApiRecipeToRecipe(r))
                const pagination = parsePaginationState(
                    response.raw.headers.get('X-Pagination'),
                    pageNumber,
                    pageSize,
                )

                this.ownRecipes = items
                this.ownRecipesPagination = pagination

                const recipeStore = useRecipeStore()
                items.forEach((item) => recipeStore.updateRecipe(item))
                recipeStore.ownRecipeIds = items.map((item) => item.id)

                return { items, pagination }
            } finally {
                this.ownRecipesLoading = false
            }
        },
    },
})
