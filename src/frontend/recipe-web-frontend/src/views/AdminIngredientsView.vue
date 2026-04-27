<script setup lang="ts">
import FormCard from '@/components/common/FormCard.vue'
import { useAuthStore } from '@/stores/authStore'
import { useAdminIngredientsStore } from '@/stores/adminIngredientsStore'
import { MapApiAllergenToEnum } from '@/types/recipe/allergen.mappers'
import { allergenList, type AllergenEnum } from '@/types/recipe/allergens'
import { storeToRefs } from 'pinia'
import { computed } from 'vue'

const auth = useAuthStore()
const adminIngredientsStore = useAdminIngredientsStore()
const allergenOptions = allergenList as readonly AllergenEnum[]
const {
    searchTerm,
    searchResults,
    searchLoading,
    saveLoading,
    selectedIngredientId,
    actionError,
    searchHint,
    form,
    isEditing,
    isSaveDisabled,
    isDeleteDisabled,
} = storeToRefs(adminIngredientsStore)

const canAccessPage = computed(() => auth.isAdmin)
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
                    Itt külön tudod kezelni a javaslatokhoz használt hozzávalókat.
                </p>
            </header>

            <div class="grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.2fr)]">
                <FormCard
                    title="Keresés és kiválasztás"
                    subtitle="Keress rá egy meglévő hozzávalóra név alapján, vagy kezdj új elemet."
                >
                    <div class="flex flex-col gap-3 sm:flex-row">
                        <input
                            data-cy="admin-ingredient-search"
                            v-model="searchTerm"
                            type="text"
                            placeholder="pl. zabpehely"
                            class="w-full rounded-lg border border-slate-300 px-4 py-2.5 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                            @keyup.enter="adminIngredientsStore.runSearch"
                        />
                        <button
                            data-cy="admin-search-button"
                            type="button"
                            :disabled="searchLoading"
                            class="rounded-lg bg-blue-600 px-5 py-2.5 text-white transition hover:bg-blue-700 disabled:opacity-50"
                            @click="adminIngredientsStore.runSearch"
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
                            data-cy="admin-ingredient-result"
                            type="button"
                            class="w-full rounded-lg border px-4 py-3 text-left transition"
                            :class="
                                selectedIngredientId === result.id
                                    ? 'border-blue-500 bg-blue-50'
                                    : 'border-slate-200 bg-white hover:border-blue-200 hover:bg-blue-50/40'
                            "
                            @click="adminIngredientsStore.loadIngredient(result)"
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
                        <div
                            v-if="isEditing"
                            class="rounded-xl border border-red-200 bg-red-50 px-4 py-4"
                        >
                            <div
                                class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between"
                            >
                                <div>
                                    <p class="text-sm font-semibold text-red-800">
                                        Kijelölt hozzávaló
                                    </p>
                                    <p class="mt-1 text-sm text-red-700">
                                        {{ form.name || 'Névtelen hozzávaló' }} jelenleg szerkesztés
                                        alatt áll.
                                    </p>
                                </div>
                                <button
                                    data-cy="admin-ingredient-delete"
                                    type="button"
                                    :disabled="isDeleteDisabled"
                                    class="rounded-lg bg-red-600 px-5 py-2.5 text-white transition hover:bg-red-700 disabled:opacity-50"
                                    @click="adminIngredientsStore.deleteIngredient"
                                >
                                    {{ saveLoading ? 'Törlés...' : 'Hozzávaló törlése' }}
                                </button>
                            </div>
                        </div>

                        <div>
                            <label
                                for="ingredient-name"
                                class="mb-2 block text-sm font-medium text-slate-700"
                            >
                                Hozzávaló neve
                            </label>
                            <input
                                id="ingredient-name"
                                data-cy="admin-ingredient-name"
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

                        <div class="flex flex-wrap gap-3">
                            <button
                                data-cy="admin-ingredient-save"
                                type="button"
                                :disabled="isSaveDisabled"
                                class="rounded-lg bg-blue-600 px-5 py-2.5 text-white transition hover:bg-blue-700 disabled:opacity-50"
                                @click="adminIngredientsStore.saveIngredient"
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
                                @click="adminIngredientsStore.resetForm"
                            >
                                {{ isEditing ? 'Új hozzávaló' : 'Űrlap ürítése' }}
                            </button>
                        </div>
                    </div>
                </FormCard>
            </div>
        </template>
    </main>
</template>
