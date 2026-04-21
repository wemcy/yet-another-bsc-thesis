<script setup lang="ts">
import FormCard from '@/components/common/FormCard.vue'
import { useAuthStore } from '@/stores/authStore'
import { ingredientApiClient } from '@/utils/recipeApiClient'
import { toErrorMessage } from '@/utils/identityErrors'
import { MapApiAllergenToEnum, MapEnumToApiAllergen } from '@/types/recipe/allergen.mappers'
import { allergenList, type AllergenEnum } from '@/types/recipe/allergens'
import type { IngredientSuggestion } from 'recipe-api-client'
import { computed, ref } from 'vue'

interface IngredientFormState {
    id: string | null
    name: string
    allergens: AllergenEnum[]
}

const auth = useAuthStore()
const allergenOptions = allergenList as readonly AllergenEnum[]

const searchTerm = ref('')
const searchResults = ref<IngredientSuggestion[]>([])
const searchLoading = ref(false)
const saveLoading = ref(false)
const selectedIngredientId = ref<string | null>(null)
const actionError = ref<string | null>(null)
const actionSuccess = ref<string | null>(null)
const searchHint = ref('Keress rá egy hozzávalóra, majd válaszd ki a szerkesztéshez.')

const form = ref<IngredientFormState>({
    id: null,
    name: '',
    allergens: [],
})

const canAccessPage = computed(() => auth.isAdmin)
const isEditing = computed(() => !!form.value.id)
const isSaveDisabled = computed(() => saveLoading.value || form.value.name.trim().length === 0)
const isDeleteDisabled = computed(() => saveLoading.value || !form.value.id)

function clearMessages() {
    actionError.value = null
    actionSuccess.value = null
}

function resetForm() {
    form.value = {
        id: null,
        name: '',
        allergens: [],
    }
    selectedIngredientId.value = null
    clearMessages()
}

function mapApiAllergensToUi(allergens: Set<Parameters<typeof MapApiAllergenToEnum>[0]>) {
    return Array.from(allergens).map((allergen) => MapApiAllergenToEnum(allergen))
}

async function runSearch() {
    const term = searchTerm.value.trim()
    clearMessages()

    if (!term) {
        searchResults.value = []
        searchHint.value = 'Adj meg legalább egy karaktert a kereséshez.'
        return
    }

    searchLoading.value = true

    try {
        searchResults.value = await ingredientApiClient.searchIngredients({ name: term })
        searchHint.value =
            searchResults.value.length === 0
                ? 'Nincs találat erre a keresésre.'
                : `${searchResults.value.length} találat érkezett.`
    } catch (error) {
        searchResults.value = []
        actionError.value = await toErrorMessage(error)
        searchHint.value = 'A keresés nem sikerült.'
    } finally {
        searchLoading.value = false
    }
}

async function loadIngredient(result: IngredientSuggestion) {
    clearMessages()
    selectedIngredientId.value = result.id

    try {
        const ingredient = await ingredientApiClient.getIngredientById({ id: result.id })
        form.value = {
            id: result.id,
            name: ingredient.name,
            allergens: mapApiAllergensToUi(ingredient.allergens),
        }
        actionSuccess.value = `"${ingredient.name}" betöltve szerkesztésre.`
    } catch (error) {
        actionError.value = await toErrorMessage(error)
    }
}

async function saveIngredient() {
    const trimmedName = form.value.name.trim()
    if (!trimmedName) {
        actionError.value = 'A hozzávaló neve kötelező.'
        actionSuccess.value = null
        return
    }

    saveLoading.value = true
    clearMessages()

    const payload = {
        name: trimmedName,
        allergens: new Set(form.value.allergens.map((allergen) => MapEnumToApiAllergen(allergen))),
    }

    try {
        if (form.value.id) {
            await ingredientApiClient.updateIngredient({
                id: form.value.id,
                createIngredientSuggestionRequest: payload,
            })
            actionSuccess.value = 'A hozzávaló módosítása sikeres volt.'
        } else {
            await ingredientApiClient.createIngredient({
                createIngredientSuggestionRequest: payload,
            })
            actionSuccess.value = 'Az új hozzávaló létrehozása sikeres volt.'
        }

        form.value.name = trimmedName
        searchTerm.value = trimmedName
        await runSearch()

        const refreshedMatch = searchResults.value.find(
            (result) => result.name.toLocaleLowerCase() === trimmedName.toLocaleLowerCase(),
        )

        if (refreshedMatch) {
            form.value.id = refreshedMatch.id
            selectedIngredientId.value = refreshedMatch.id
        }
    } catch (error) {
        actionError.value = await toErrorMessage(error)
    } finally {
        saveLoading.value = false
    }
}

async function deleteIngredient() {
    if (!form.value.id) return

    saveLoading.value = true
    clearMessages()

    try {
        await ingredientApiClient.deleteIngredient({ id: form.value.id })
        const deletedIngredientName = form.value.name.trim()

        resetForm()
        actionSuccess.value = deletedIngredientName
            ? `"${deletedIngredientName}" hozzávaló törölve lett.`
            : 'A hozzávaló törölve lett.'

        if (searchTerm.value.trim()) {
            await runSearch()
        } else {
            searchResults.value = []
            searchHint.value = 'Keress rá egy hozzávalóra, majd válaszd ki a szerkesztéshez.'
        }
    } catch (error) {
        actionError.value = await toErrorMessage(error)
    } finally {
        saveLoading.value = false
    }
}
</script>

<template>
    <main class="mx-auto max-w-7xl px-4 py-8">
        <div
            v-if="!canAccessPage"
            class="mx-auto mt-10 max-w-xl rounded-xl border border-dashed border-gray-300 bg-white p-8 text-center shadow-sm"
        >
            <h1 class="text-2xl font-semibold text-gray-900">
                Nincs hozzáférésed ehhez az oldalhoz
            </h1>
            <p class="mt-3 text-gray-600">
                Az összetevők adminisztrációja csak admin felhasználók számára érhető el.
            </p>
        </div>

        <template v-else>
            <header class="mb-8">
                <p class="text-sm font-semibold uppercase tracking-[0.2em] text-blue-600">
                    Admin felület
                </p>
                <h1 class="mt-2 text-3xl font-bold text-slate-900">Hozzávalók kezelése</h1>
                <p class="mt-3 max-w-3xl text-slate-600">
                    Itt külön tudod kezelni a javaslatokhoz használt hozzávalókat. A meglévő keresős
                    komponenseket ez az oldal nem módosítja.
                </p>
            </header>

            <div class="grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.2fr)]">
                <FormCard
                    title="Keresés és kiválasztás"
                    subtitle="Keress rá egy meglévő hozzávalóra név alapján, vagy kezdj új elemet."
                >
                    <div class="flex flex-col gap-3 sm:flex-row">
                        <input
                            v-model="searchTerm"
                            type="text"
                            placeholder="pl. zabpehely"
                            class="w-full rounded-lg border border-slate-300 px-4 py-2.5 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                            @keyup.enter="runSearch"
                        />
                        <button
                            type="button"
                            :disabled="searchLoading"
                            class="rounded-lg bg-blue-600 px-5 py-2.5 text-white transition hover:bg-blue-700 disabled:opacity-50"
                            @click="runSearch"
                        >
                            {{ searchLoading ? 'Keresés...' : 'Keresés' }}
                        </button>
                    </div>

                    <p class="mt-4 text-sm text-slate-500">{{ searchHint }}</p>

                    <div
                        v-if="searchResults.length > 0"
                        class="mt-4 space-y-3 rounded-xl border border-slate-200 bg-slate-50 p-3"
                    >
                        <button
                            v-for="result in searchResults"
                            :key="result.id"
                            type="button"
                            class="w-full rounded-lg border px-4 py-3 text-left transition"
                            :class="
                                selectedIngredientId === result.id
                                    ? 'border-blue-500 bg-blue-50'
                                    : 'border-slate-200 bg-white hover:border-blue-200 hover:bg-blue-50/40'
                            "
                            @click="loadIngredient(result)"
                        >
                            <div class="font-semibold text-slate-900">{{ result.name }}</div>
                            <div class="mt-1 text-sm text-slate-500">
                                {{
                                    Array.from(result.allergens).length > 0
                                        ? Array.from(result.allergens)
                                              .map((allergen) => MapApiAllergenToEnum(allergen))
                                              .join(', ')
                                        : 'Nincs megadott allergén'
                                }}
                            </div>
                        </button>
                    </div>
                </FormCard>

                <FormCard
                    :title="isEditing ? 'Hozzávaló szerkesztése' : 'Új hozzávaló létrehozása'"
                    subtitle="A név és az allergének módosíthatók ezen a felületen."
                >
                    <div class="space-y-6">
                        <div>
                            <label
                                for="ingredient-name"
                                class="mb-2 block text-sm font-medium text-slate-700"
                            >
                                Hozzávaló neve
                            </label>
                            <input
                                id="ingredient-name"
                                v-model="form.name"
                                type="text"
                                placeholder="Add meg a hozzávaló nevét"
                                class="w-full rounded-lg border border-slate-300 px-4 py-2.5 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                            />
                        </div>

                        <div>
                            <p class="mb-3 text-sm font-medium text-slate-700">Allergének</p>
                            <div class="grid gap-3 sm:grid-cols-2">
                                <label
                                    v-for="allergen in allergenOptions"
                                    :key="allergen"
                                    class="flex items-center gap-3 rounded-lg border border-slate-200 bg-white px-4 py-3 text-sm text-slate-700"
                                >
                                    <input
                                        v-model="form.allergens"
                                        type="checkbox"
                                        :value="allergen"
                                        class="h-4 w-4 rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                                    />
                                    <span>{{ allergen }}</span>
                                </label>
                            </div>
                        </div>

                        <p
                            v-if="actionError"
                            class="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
                        >
                            {{ actionError }}
                        </p>
                        <p
                            v-else-if="actionSuccess"
                            class="rounded-lg border border-emerald-200 bg-emerald-50 px-4 py-3 text-sm text-emerald-700"
                        >
                            {{ actionSuccess }}
                        </p>

                        <div class="flex flex-wrap gap-3">
                            <button
                                type="button"
                                :disabled="isSaveDisabled"
                                class="rounded-lg bg-blue-600 px-5 py-2.5 text-white transition hover:bg-blue-700 disabled:opacity-50"
                                @click="saveIngredient"
                            >
                                {{
                                    saveLoading
                                        ? 'Mentés...'
                                        : isEditing
                                          ? 'Módosítás mentése'
                                          : 'Hozzávaló létrehozása'
                                }}
                            </button>
                            <button
                                type="button"
                                class="rounded-lg border border-slate-300 px-5 py-2.5 text-slate-700 transition hover:bg-slate-50"
                                @click="resetForm"
                            >
                                Új hozzávaló
                            </button>
                            <button
                                type="button"
                                class="rounded-lg border border-slate-300 px-5 py-2.5 text-slate-700 transition hover:bg-slate-50"
                                @click="resetForm"
                            >
                                Űrlap ürítése
                            </button>
                            <button
                                v-if="isEditing"
                                type="button"
                                :disabled="isDeleteDisabled"
                                class="rounded-lg bg-red-600 px-5 py-2.5 text-white transition hover:bg-red-700 disabled:opacity-50"
                                @click="deleteIngredient"
                            >
                                {{ saveLoading ? 'Törlés...' : 'Hozzávaló törlése' }}
                            </button>
                        </div>
                    </div>
                </FormCard>
            </div>
        </template>
    </main>
</template>
