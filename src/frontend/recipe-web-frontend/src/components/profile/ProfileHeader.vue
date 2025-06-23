<template>
    <section
        class="flex flex-col items-center gap-4 bg-white rounded-xl shadow p-6 w-full max-w-2xl mx-auto"
    >
        <!-- Avatar -->
        <div class="flex flex-col items-center">
            <ProfileAvatar :src="imageUrl || profile.avatarUrl" :alt="profile.name" size="lg" />
            <div v-if="editing" class="mt-2">
                <input type="file" accept="image/*" @change="$emit('imageChange', $event)" />
            </div>
        </div>
        <!-- Név -->
        <div class="w-full">
            <label class="block font-semibold">Név</label>
            <input
                v-if="editing"
                v-model="localProfile.name"
                class="border rounded px-3 py-1 w-full"
                @input="$emit('updateProfile', localProfile)"
            />
            <p v-else>{{ profile.name }}</p>
            <p v-if="errors?.name" class="text-red-600 text-sm">{{ errors.name }}</p>
        </div>
        <!-- Email -->
        <div class="w-full">
            <label class="block font-semibold">Email</label>
            <input
                v-if="editing"
                v-model="localProfile.email"
                class="border rounded px-3 py-1 w-full"
                @input="$emit('updateProfile', localProfile)"
            />
            <p v-else>{{ profile.email }}</p>
            <p v-if="errors?.email" class="text-red-600 text-sm">{{ errors.email }}</p>
        </div>
        <!-- Jelszó -->
        <div class="w-full">
            <label class="block font-semibold">Jelszó</label>
            <input
                v-if="editing"
                v-model="localProfile.password"
                type="password"
                class="border rounded px-3 py-1 w-full"
                placeholder="Új jelszó (ha változtatni szeretnél)"
                @input="$emit('updateProfile', localProfile)"
            />
            <p v-if="errors?.password" class="text-red-600 text-sm">{{ errors.password }}</p>

            <label v-if="editing" class="block font-semibold">Jelszó megerősítése</label>
            <input
                v-if="editing"
                :value="passwordConfirm"
                @input="onPasswordConfirmInput"
                type="password"
                class="border rounded px-3 py-1 w-full"
                placeholder="Új jelszó ismét"
            />
            <p v-if="errors?.passwordConfirm" class="text-red-600 text-sm">
                {{ errors.passwordConfirm }}
            </p>
            <!-- Csak NEM szerkesztő módban mutass csillagot -->
            <p v-else-if="!editing">********</p>
        </div>
        <!-- Regisztráció -->
        <div class="w-full">
            <label class="block font-semibold">Regisztráció dátuma</label>
            <p>{{ profile.registered }}</p>
        </div>
        <!-- Gombok -->
        <div class="flex gap-2 mt-4 w-full justify-end">
            <button
                v-if="!editing"
                @click="$emit('edit')"
                class="border px-4 py-1 rounded hover:bg-gray-100"
            >
                Szerkesztés
            </button>
            <template v-else>
                <button @click="$emit('cancel')" class="border px-4 py-1 rounded hover:bg-gray-100">
                    Mégse
                </button>
                <button
                    @click="$emit('save')"
                    class="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
                >
                    Mentés
                </button>
            </template>
        </div>
    </section>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import ProfileAvatar from '@/components/profile/ProfileAvatar.vue'
import type { User } from '@/types/profile/user'
import type { ProfileError } from '@/types/profile/profile'

const props = defineProps<{
    profile: User
    editing: boolean
    errors?: ProfileError
    passwordConfirm?: string
    imageUrl?: string | null
}>()

const localProfile = ref<User>({ ...props.profile })
const emit = defineEmits<{
    (e: 'updatePasswordConfirm', value: string): void
    (e: 'updateProfile', profile: User): void
    (e: 'edit'): void
    (e: 'cancel'): void
    (e: 'save'): void
    (e: 'imageChange', event: Event): void
}>()

watch(
    () => props.profile,
    (newVal) => {
        localProfile.value = { ...newVal }
    },
    { deep: true },
)
function onPasswordConfirmInput(e: Event) {
    const value = (e.target as HTMLInputElement)?.value ?? ''
    emit('updatePasswordConfirm', value)
}
</script>
