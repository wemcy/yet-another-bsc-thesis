import type { PaginatedRecipes, PaginationState, Recipe } from '@/types/recipe/recipe'
import { MapApiRecipeToRecipe } from '@/types/recipe/recipe.mappers'
import { createDefaultPaginationState, parsePaginationState } from '@/utils/pagination'
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import { defineStore } from 'pinia'
import { useRecipeStore } from './recipeStore'
import type { Allergen } from 'recipe-api-client'

interface RecipePaginationState {
    allRecipes: Recipe[]
    allRecipesPagination: PaginationState
    allRecipesLoading: boolean
    ownRecipes: Recipe[]
    ownRecipesPagination: PaginationState
    ownRecipesLoading: boolean
}

export const useRecipePaginationStore = defineStore('recipePagination', {
    state: (): RecipePaginationState => ({
        allRecipes: [],
        allRecipesPagination: createDefaultPaginationState(0, 27),
        allRecipesLoading: false,
        ownRecipes: [],
        ownRecipesPagination: createDefaultPaginationState(0, 27),
        ownRecipesLoading: false,
    }),
    actions: {
        async loadAllRecipesPage(
            pageNumber: number,
            pageSize: number,
            title = '',
            includeAllergens: Allergen[] = [],
            excludeAllergens: Allergen[] = [],
        ): Promise<PaginatedRecipes> {
            this.allRecipesLoading = true
            try {
                const response = await api.listRecipesRaw({
                    page: pageNumber,
                    pageSize,
                    title: title.trim().length > 0 ? title.trim() : undefined,
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
