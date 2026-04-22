import { defineStore } from 'pinia'
import { ingredientApiClient } from '@/utils/recipeApiClient'
import { toErrorMessage } from '@/utils/identityErrors'
import { MapApiAllergenToEnum, MapEnumToApiAllergen } from '@/types/recipe/allergen.mappers'
import type { AllergenEnum } from '@/types/recipe/allergens'
import type { IngredientSuggestion } from 'recipe-api-client'

interface IngredientFormState {
    id: string | null
    name: string
    allergens: AllergenEnum[]
}

interface AdminIngredientsState {
    searchTerm: string
    searchResults: IngredientSuggestion[]
    searchLoading: boolean
    saveLoading: boolean
    selectedIngredientId: string | null
    actionError: string | null
    actionSuccess: string | null
    searchHint: string
    form: IngredientFormState
}

const createEmptyForm = (): IngredientFormState => ({
    id: null,
    name: '',
    allergens: [],
})

const defaultSearchHint = 'Keress rá egy hozzávalóra, majd válaszd ki a szerkesztéshez.'

function mapApiAllergensToUi(allergens: Set<Parameters<typeof MapApiAllergenToEnum>[0]>) {
    return Array.from(allergens).map((allergen) => MapApiAllergenToEnum(allergen))
}

export const useAdminIngredientsStore = defineStore('adminIngredients', {
    state: (): AdminIngredientsState => ({
        searchTerm: '',
        searchResults: [],
        searchLoading: false,
        saveLoading: false,
        selectedIngredientId: null,
        actionError: null,
        actionSuccess: null,
        searchHint: defaultSearchHint,
        form: createEmptyForm(),
    }),

    getters: {
        isEditing: (state) => !!state.form.id,
        isSaveDisabled: (state) => state.saveLoading || state.form.name.trim().length === 0,
        isDeleteDisabled: (state) => state.saveLoading || !state.form.id,
    },

    actions: {
        clearMessages() {
            this.actionError = null
            this.actionSuccess = null
        },
        resetForm() {
            this.form = createEmptyForm()
            this.selectedIngredientId = null
            this.clearMessages()
        },
        async runSearch() {
            const term = this.searchTerm.trim()
            this.clearMessages()

            if (!term) {
                this.searchResults = []
                this.searchHint = 'Adj meg legalább egy karaktert a kereséshez.'
                return
            }

            this.searchLoading = true

            try {
                this.searchResults = await ingredientApiClient.searchIngredients({ name: term })
                this.searchHint =
                    this.searchResults.length === 0
                        ? 'Nincs találat erre a keresésre.'
                        : `${this.searchResults.length} találat érkezett.`
            } catch (error) {
                this.searchResults = []
                this.actionError = await toErrorMessage(error)
                this.searchHint = 'A keresés nem sikerült.'
            } finally {
                this.searchLoading = false
            }
        },
        async loadIngredient(result: IngredientSuggestion) {
            this.clearMessages()
            this.selectedIngredientId = result.id

            try {
                const ingredient = await ingredientApiClient.getIngredientById({ id: result.id })
                this.form = {
                    id: result.id,
                    name: ingredient.name,
                    allergens: mapApiAllergensToUi(ingredient.allergens),
                }
                this.actionSuccess = `"${ingredient.name}" betöltve szerkesztésre.`
            } catch (error) {
                this.actionError = await toErrorMessage(error)
            }
        },
        async saveIngredient() {
            const trimmedName = this.form.name.trim()
            if (!trimmedName) {
                this.actionError = 'A hozzávaló neve kötelező.'
                this.actionSuccess = null
                return
            }

            this.saveLoading = true
            this.clearMessages()

            const payload = {
                name: trimmedName,
                allergens: new Set(
                    this.form.allergens.map((allergen) => MapEnumToApiAllergen(allergen)),
                ),
            }

            try {
                if (this.form.id) {
                    await ingredientApiClient.updateIngredient({
                        id: this.form.id,
                        createIngredientSuggestionRequest: payload,
                    })
                    this.actionSuccess = 'A hozzávaló módosítása sikeres volt.'
                } else {
                    await ingredientApiClient.createIngredient({
                        createIngredientSuggestionRequest: payload,
                    })
                    this.actionSuccess = 'Az új hozzávaló létrehozása sikeres volt.'
                }

                this.form.name = trimmedName
                this.searchTerm = trimmedName
                await this.runSearch()

                const refreshedMatch = this.searchResults.find(
                    (result) => result.name.toLocaleLowerCase() === trimmedName.toLocaleLowerCase(),
                )

                if (refreshedMatch) {
                    this.form.id = refreshedMatch.id
                    this.selectedIngredientId = refreshedMatch.id
                }
            } catch (error) {
                this.actionError = await toErrorMessage(error)
            } finally {
                this.saveLoading = false
            }
        },
        async deleteIngredient() {
            if (!this.form.id) return

            this.saveLoading = true
            this.clearMessages()

            try {
                await ingredientApiClient.deleteIngredient({ id: this.form.id })
                const deletedIngredientName = this.form.name.trim()

                this.resetForm()
                this.actionSuccess = deletedIngredientName
                    ? `"${deletedIngredientName}" hozzávaló törölve lett.`
                    : 'A hozzávaló törölve lett.'

                if (this.searchTerm.trim()) {
                    await this.runSearch()
                } else {
                    this.searchResults = []
                    this.searchHint = defaultSearchHint
                }
            } catch (error) {
                this.actionError = await toErrorMessage(error)
            } finally {
                this.saveLoading = false
            }
        },
    },
})
