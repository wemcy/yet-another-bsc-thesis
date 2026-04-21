<template>
    <div>
        <Combobox v-model="selectedRecipe" @update:modelValue="handleSelect">
            <ComboboxInput
                :displayValue="() => ingredientName"
                @change="onInputChange"
                autocomplete="off"
                placeholder="Keresés receptek között..."
                class="w-full px-4 py-2"
            />

            <ComboboxOptions
                v-if="shouldShowDropdown"
                class="absolute z-50 mt-2 max-h-96 w-full overflow-auto p-1 shadow-xl"
            >
                <li
                    v-if="isLoading"
                    class="flex items-center gap-3 px-3 py-3 text-sm text-gray-500"
                >
                    <span
                        class="h-4 w-4 animate-spin rounded-full border-2 border-gray-300 border-t-blue-500"
                    />
                    <span>Keresés...</span>
                </li>

                <li v-else-if="errorMessage" class="px-3 py-2 text-sm text-red-600">
                    {{ errorMessage }}
                </li>

                <li v-else-if="searchResults.length === 0" class="px-3 py-2 text-sm text-gray-500">
                    Nincs találat.
                </li>

                <ComboboxOption
                    v-for="ingredient in searchResults"
                    v-else
                    :key="ingredient.id"
                    :value="ingredient"
                    v-slot="{ active }"
                >
                    <div
                        :class="[
                            'cursor-pointer rounded-lg px-3 py-2 text-sm transition',
                            active ? 'bg-blue-50 text-blue-800' : 'text-gray-800',
                        ]"
                    >
                        {{ ingredient.name }}
                    </div>
                </ComboboxOption>
            </ComboboxOptions>
        </Combobox>
    </div>
</template>

<script setup lang="ts">
import { Combobox, ComboboxInput, ComboboxOption, ComboboxOptions } from '@headlessui/vue'
import { useDebounceFn } from '@vueuse/core'

import api from '@/utils/recipeApiClient'
import { computed, ref, watch } from 'vue'
import type { IngredientSuggestion } from 'recipe-api-client'

const selectedRecipe = ref<IngredientSuggestion | null>(null)
const searchResults = ref<IngredientSuggestion[]>([])
const ingredientName = ref('')
const isLoading = ref(false)
const errorMessage = ref('')

let latestRequestId = 0

const shouldShowDropdown = computed(() => {
    return (
        ingredientName.value.trim().length >= 2 &&
        (isLoading.value || !!errorMessage.value || searchResults.value.length >= 0)
    )
})

const runSearch = useDebounceFn(async (term: string) => {
    if (term.length < 2) {
        searchResults.value = []
        isLoading.value = false
        errorMessage.value = ''
        return
    }

    const requestId = ++latestRequestId
    isLoading.value = true
    errorMessage.value = ''

    try {
        const response = await api.ingredients.searchIngredients({ name: term })
        if (requestId !== latestRequestId) return
        searchResults.value = response.slice(0, 8)
    } catch {
        if (requestId !== latestRequestId) return
        searchResults.value = []
        errorMessage.value = 'A keresés most nem elérhető.'
    } finally {
        if (requestId === latestRequestId) {
            isLoading.value = false
        }
    }
}, 250)

watch(
    ingredientName,
    (value) => {
        void runSearch(value.trim())
    },
    { immediate: true },
)

function onInputChange(event: Event) {
    if (!(event.target instanceof HTMLInputElement)) return
    ingredientName.value = event.target.value
}

async function handleSelect(recipe: IngredientSuggestion | null) {
    if (!recipe) return

    selectedRecipe.value = null
    ingredientName.value = ''
    searchResults.value = []
}
</script>
