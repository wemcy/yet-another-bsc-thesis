<template>
    <div class="relative w-full">
        <Combobox v-model="selectedRecipe" @update:modelValue="handleSelect">
            <ComboboxInput
                :displayValue="() => searchTerm"
                @change="onInputChange"
                autocomplete="off"
                placeholder="Keresés receptek között..."
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />

            <ComboboxOptions
                v-if="shouldShowDropdown"
                class="absolute z-50 mt-2 max-h-96 w-full overflow-auto rounded-xl border border-gray-200 bg-white p-1 shadow-xl"
            >
                <li v-if="isLoading" class="px-3 py-2 text-sm text-gray-500">Keresés...</li>
                <li v-else-if="errorMessage" class="px-3 py-2 text-sm text-red-600">
                    {{ errorMessage }}
                </li>
                <li v-else-if="searchResults.length === 0" class="px-3 py-2 text-sm text-gray-500">
                    Nincs találat.
                </li>
                <ComboboxOption
                    v-for="recipe in searchResults"
                    v-else
                    :key="recipe.id"
                    :value="recipe"
                    v-slot="{ active }"
                >
                    <div
                        :class="[
                            'cursor-pointer rounded-lg px-3 py-2 text-sm transition',
                            active ? 'bg-blue-50 text-blue-800' : 'text-gray-800',
                        ]"
                    >
                        {{ recipe.title }}
                    </div>
                </ComboboxOption>
            </ComboboxOptions>
        </Combobox>
    </div>
</template>

<script setup lang="ts">
import { recipeApiClient as api } from '@/utils/recipeApiClient'
import { Combobox, ComboboxInput, ComboboxOption, ComboboxOptions } from '@headlessui/vue'
import { useDebounceFn } from '@vueuse/core'
import type { RecipeSummary } from 'recipe-api-client'
import { computed, ref, watch } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const searchTerm = ref('')
const selectedRecipe = ref<RecipeSummary | null>(null)
const searchResults = ref<RecipeSummary[]>([])
const isLoading = ref(false)
const errorMessage = ref('')

let latestRequestId = 0

const shouldShowDropdown = computed(() => {
    return (
        searchTerm.value.trim().length >= 2 &&
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
        const response = await api.searchRecipes({ title: term })
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
    searchTerm,
    (value) => {
        void runSearch(value.trim())
    },
    { immediate: true },
)

function onInputChange(event: Event) {
    const target = event.target as HTMLInputElement
    searchTerm.value = target.value
}

async function handleSelect(recipe: RecipeSummary | null) {
    if (!recipe) return

    selectedRecipe.value = null
    searchTerm.value = ''
    searchResults.value = []
    await router.push({ name: 'Recipe', params: { id: recipe.id } })
}
</script>
