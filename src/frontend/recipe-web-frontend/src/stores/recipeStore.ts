import type {
    Comment,
    NewRecipeDraft,
    PaginatedComments,
    PaginationState,
    Recipe,
    RecipeState,
} from '@/types/recipe/recipe'
import { MapApiRecipeToRecipe, MapRecipeToApiRecipe } from '@/types/recipe/recipe.mappers'
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import { defineStore } from 'pinia'

const commentsPageSize = 25

const createEmptyNewRecipeDraft = (): NewRecipeDraft => ({
    title: '',
    description: '',
    ingredients: [{ quantity: 0, unitOfMeasurement: '', name: '' }],
    steps: [''],
    selectedAllergens: [],
})

const getNewRecipeDraftStorageKey = (userId?: string | null) =>
    `new-recipe-draft:${userId ?? 'guest'}`

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

const mapApiCommentToComment = (apiComment: {
    id: string
    content: string
    createdAt: Date
    author: string
    authorId?: string
}): Comment => ({
    id: apiComment.id,
    authorId: apiComment.authorId ?? '',
    authorName: apiComment.author,
    content: apiComment.content,
    createdAt: apiComment.createdAt,
})

export const useRecipeStore = defineStore('recipe', {
    state: (): RecipeState => ({
        recipes: [],
        featuredRecipeId: null,
        showcaseRecipesIds: [],
        ownRecipeIds: [] as string[],
        newRecipeDraft: createEmptyNewRecipeDraft(),
        showcaseRecipesLoading: false,
        featuredRecipeLoading: false,
        commentsByRecipeId: {},
        commentsPaginationByRecipeId: {},
        commentsLoadingByRecipeId: {},
    }),

    getters: {
        getById: (state) => (id: string) => state.recipes.find((r) => r.id === id),
        featuredRecipe: (state) => state.recipes.find((r) => r.id === state.featuredRecipeId),
        showcaseRecipes: (state) =>
            state.recipes.filter((r) => state.showcaseRecipesIds.includes(r.id)),
        getCommentsByRecipeId: (state) => (id: string) => state.commentsByRecipeId[id] ?? [],
        getCommentsPaginationByRecipeId: (state) => (id: string) =>
            state.commentsPaginationByRecipeId[id] ??
            createDefaultPaginationState(0, commentsPageSize),
        isCommentsLoadingByRecipeId: (state) => (id: string) =>
            state.commentsLoadingByRecipeId[id] ?? false,
        ownRecipes: (state) => state.recipes.filter((r) => state.ownRecipeIds.includes(r.id)),
    },

    actions: {
        loadNewRecipeDraft(userId?: string | null) {
            const storageKey = getNewRecipeDraftStorageKey(userId)
            const rawDraft = localStorage.getItem(storageKey)

            if (!rawDraft) {
                this.newRecipeDraft = createEmptyNewRecipeDraft()
                return this.newRecipeDraft
            }

            try {
                const parsed = JSON.parse(rawDraft) as Partial<NewRecipeDraft>
                this.newRecipeDraft = {
                    title: typeof parsed.title === 'string' ? parsed.title : '',
                    description: typeof parsed.description === 'string' ? parsed.description : '',
                    ingredients:
                        Array.isArray(parsed.ingredients) && parsed.ingredients.length > 0
                            ? parsed.ingredients
                            : [{ quantity: 0, unitOfMeasurement: '', name: '' }],
                    steps:
                        Array.isArray(parsed.steps) && parsed.steps.length > 0
                            ? parsed.steps
                            : [''],
                    selectedAllergens: Array.isArray(parsed.selectedAllergens)
                        ? parsed.selectedAllergens
                        : [],
                }
            } catch {
                this.newRecipeDraft = createEmptyNewRecipeDraft()
                localStorage.removeItem(storageKey)
            }

            return this.newRecipeDraft
        },
        saveNewRecipeDraft(draft: NewRecipeDraft, userId?: string | null) {
            this.newRecipeDraft = draft
            localStorage.setItem(getNewRecipeDraftStorageKey(userId), JSON.stringify(draft))
        },
        clearNewRecipeDraft(userId?: string | null) {
            this.newRecipeDraft = createEmptyNewRecipeDraft()
            localStorage.removeItem(getNewRecipeDraftStorageKey(userId))
        },
        async addRecipe(recipe: Omit<Recipe, 'id'>) {
            const response = await api.createRecipe({
                createRecipeRequest: MapRecipeToApiRecipe(recipe),
            })
            return response.id
        },
        async updateRating(id: string, rating: number) {
            const recipe = this.recipes.find((r) => r.id === id)
            const previousRating = recipe?.rating

            if (recipe) recipe.rating = rating

            try {
                await api.rateRecipe({
                    id,
                    rateRecipeRequest: { rating },
                })
            } catch (error) {
                if (recipe && previousRating !== undefined) {
                    recipe.rating = previousRating
                }
                throw error
            }

            await this.fetchRecipeById(id)
        },
        updateRecipe(recipe: Recipe) {
            const idx = this.recipes.findIndex((r) => r.id === recipe.id)
            if (idx !== -1) this.recipes[idx] = recipe
            else this.recipes.push(recipe)
        },

        async refreshRecipes() {
            await api.listRecipes({ page: 0, pageSize: 100 }).then((response) => {
                response.forEach((apiRecipe) => {
                    const recipe = MapApiRecipeToRecipe(apiRecipe)
                    this.updateRecipe(recipe)
                })
            })
        },
        async fetchRecipeById(id: string) {
            const recipe = await api
                .getRecipeById({ id })
                .then((apiRecipe) => MapApiRecipeToRecipe(apiRecipe))
            this.updateRecipe(recipe)
            await this.fetchRecipeCommentsPage(id, 0, commentsPageSize)
        },
        async fetchShowcaseRecipes() {
            this.showcaseRecipesLoading = true
            try {
                await api.listShowcaseRecipes().then((response) => {
                    this.showcaseRecipesIds = response.map((r) => r.id)
                    response.forEach((apiRecipe) => {
                        const recipe = MapApiRecipeToRecipe(apiRecipe)
                        this.updateRecipe(recipe)
                    })
                })
            } finally {
                this.showcaseRecipesLoading = false
            }
        },
        async fetchFeaturedRecipe() {
            this.featuredRecipeLoading = true
            try {
                await api.getFeaturedRecipe().then((response) => {
                    this.featuredRecipeId = response.id
                    const recipe = MapApiRecipeToRecipe(response)
                    this.updateRecipe(recipe)
                })
            } finally {
                this.featuredRecipeLoading = false
            }
        },
        async setFeaturedRecipe(id: string) {
            this.featuredRecipeLoading = true
            try {
                await api.updateFeaturedRecipe({
                    updateFeaturedRecipeRequest: { recipeId: id },
                })
                this.featuredRecipeId = id
                await this.fetchFeaturedRecipe()
            } finally {
                this.featuredRecipeLoading = false
            }
        },
        async updateImage(id: string, image: File) {
            await api.updateRecipeImage({ id, image })
        },
        async updateRecipeById(id: string, recipe: Omit<Recipe, 'id'>, image?: File | null) {
            const response = await api.updateRecipeById({
                id,
                createRecipeRequest: MapRecipeToApiRecipe(recipe),
            })
            const updated = MapApiRecipeToRecipe(response)
            this.updateRecipe(updated)
            if (image) {
                await this.updateImage(id, image)
            }
            return updated
        },
        async fetchOwnRecipes(authorId: string) {
            const response = await api.getRecipesByAuthorId({
                id: authorId,
                page: 0,
                pageSize: 100,
            })
            const mapped = response.map((r) => MapApiRecipeToRecipe(r))
            mapped.forEach((r) => this.updateRecipe(r))
            this.ownRecipeIds = mapped.map((r) => r.id)
        },
        async addRecipeComment(recipeId: string, content: string) {
            await api.addRecipeCommentRaw({
                id: recipeId,
                addRecipeCommentRequest: { content },
            })
        },
        async deleteRecipeComment(recipeId: string, commentId: string) {
            await api.deleteRecipeComment({ recipeId, commentId })
        },
        async deleteRecipe(id: string) {
            await api.deleteRecipeById({ id })
            this.recipes = this.recipes.filter((r) => r.id !== id)
        },
        async fetchRecipeCommentsPage(
            recipeId: string,
            page: number,
            pageSize: number = commentsPageSize,
        ): Promise<PaginatedComments> {
            this.commentsLoadingByRecipeId = {
                ...this.commentsLoadingByRecipeId,
                [recipeId]: true,
            }

            try {
                const response = await api.listRecipeCommentsRaw({ id: recipeId, page, pageSize })
                const body = await response.value()
                const items = body.map((comment) => mapApiCommentToComment(comment))
                const pagination = parsePaginationState(
                    response.raw.headers.get('X-Pagination'),
                    page,
                    pageSize,
                )

                this.commentsByRecipeId = {
                    ...this.commentsByRecipeId,
                    [recipeId]: items,
                }
                this.commentsPaginationByRecipeId = {
                    ...this.commentsPaginationByRecipeId,
                    [recipeId]: pagination,
                }

                return {
                    items,
                    pagination,
                }
            } finally {
                this.commentsLoadingByRecipeId = {
                    ...this.commentsLoadingByRecipeId,
                    [recipeId]: false,
                }
            }
        },
    },
})
