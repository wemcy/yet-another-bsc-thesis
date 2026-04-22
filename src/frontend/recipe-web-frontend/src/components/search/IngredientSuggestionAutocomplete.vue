<template>
    <div class="relative">
        <Combobox v-model="selectedIngredient" @update:modelValue="handleSelect">
            <ComboboxInput
                v-bind="attrs"
                :displayValue="displayValue"
                @change="onInputChange"
                autocomplete="off"
                :placeholder="placeholder"
                class="w-full border rounded px-2 py-1 bg-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400"
            />

            <ComboboxOptions
                v-if="shouldShowDropdown"
                class="absolute z-50 mt-2 max-h-96 w-full overflow-auto rounded-xl border border-slate-200 bg-white p-2 shadow-xl"
            >
                <li
                    v-if="isLoading"
                    class="flex min-h-11 items-center gap-3 rounded-lg px-3 py-3 text-sm text-gray-500"
                >
                    <span
                        class="h-4 w-4 animate-spin rounded-full border-2 border-gray-300 border-t-blue-500"
                    />
                    <span>Keresés...</span>
                </li>

                <li
                    v-else-if="errorMessage"
                    class="min-h-11 rounded-lg px-3 py-3 text-sm text-red-600"
                >
                    {{ errorMessage }}
                </li>

                <li
                    v-else-if="searchResults.length === 0"
                    class="min-h-11 rounded-lg px-3 py-3 text-sm text-gray-500"
                >
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
                            'min-h-11 cursor-pointer rounded-lg px-3 py-3 text-sm transition',
                            active ? 'bg-blue-50 text-blue-800' : 'text-gray-800',
                        ]"
                    >
                        <div class="font-medium">{{ ingredient.name }}</div>
                        <div
                            v-if="ingredient.allergens.size > 0"
                            class="mt-1 text-xs text-slate-500"
                        >
                            {{ formatAllergens(ingredient) }}
                        </div>
                    </div>
                </ComboboxOption>
            </ComboboxOptions>
        </Combobox>
    </div>
</template>

<script setup lang="ts">
defineOptions({
    inheritAttrs: false,
})

import { Combobox, ComboboxInput, ComboboxOption, ComboboxOptions } from '@headlessui/vue'
import { useDebounceFn } from '@vueuse/core'

import api from '@/utils/recipeApiClient'
import { MapApiAllergenToEnum } from '@/types/recipe/allergen.mappers'
import type { AllergenEnum } from '@/types/recipe/allergens'
import { computed, ref, useAttrs, watch } from 'vue'
import type { IngredientSuggestion } from 'recipe-api-client'

const props = withDefaults(
    defineProps<{
        modelValue: string
        allergens?: AllergenEnum[]
        placeholder?: string
    }>(),
    {
        allergens: () => [],
        placeholder: 'Hozzávaló keresése...',
    },
)

const emit = defineEmits<{
    'update:modelValue': [value: string]
    'update:allergens': [value: AllergenEnum[]]
    select: [value: IngredientSuggestion]
}>()

const attrs = useAttrs()
const selectedIngredient = ref<IngredientSuggestion | null>(null)
const searchResults = ref<IngredientSuggestion[]>([])
const inputValue = ref(props.modelValue ?? '')
const isLoading = ref(false)
const errorMessage = ref('')

let latestRequestId = 0

const shouldShowDropdown = computed(() => {
    return (
        inputValue.value.trim().length >= 2 &&
        (isLoading.value || !!errorMessage.value || searchResults.value.length >= 0)
    )
})

watch(
    () => props.modelValue,
    (value) => {
        if (value !== inputValue.value) {
            inputValue.value = value ?? ''
        }
    },
)

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
    inputValue,
    (value) => {
        void runSearch(value.trim())
    },
    { immediate: true },
)

function displayValue() {
    return inputValue.value
}

function onInputChange(event: Event) {
    if (!(event.target instanceof HTMLInputElement)) return
    inputValue.value = event.target.value
    emit('update:modelValue', inputValue.value)
}

function mapIngredientAllergens(ingredient: IngredientSuggestion) {
    return Array.from(ingredient.allergens).map((allergen) => MapApiAllergenToEnum(allergen))
}

function formatAllergens(ingredient: IngredientSuggestion) {
    return mapIngredientAllergens(ingredient).join(', ')
}

async function handleSelect(ingredient: IngredientSuggestion | null) {
    if (!ingredient) return

    const mappedAllergens = mapIngredientAllergens(ingredient)
    selectedIngredient.value = ingredient
    inputValue.value = ingredient.name
    emit('update:modelValue', ingredient.name)
    emit('update:allergens', mappedAllergens)
    emit('select', ingredient)
    searchResults.value = []
    errorMessage.value = ''
}
</script>
