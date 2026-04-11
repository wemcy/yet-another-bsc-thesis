<template>
    <form class="space-y-4" @submit.prevent="onSubmit">
        <div>
            <label for="register-name" class="block text-sm font-medium text-slate-700">Név</label>
            <input
                id="register-name"
                v-model.trim="name"
                type="text"
                autocomplete="name"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Teljes neved"
            />
            <p v-if="errors.name" class="text-sm text-red-600 mt-1">{{ errors.name }}</p>
        </div>

        <div>
            <label for="register-email" class="block text-sm font-medium text-slate-700"
                >E-mail</label
            >
            <input
                id="register-email"
                v-model.trim="email"
                type="email"
                autocomplete="email"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="te@domain.hu"
            />
            <p v-if="errors.email" class="text-sm text-red-600 mt-1">{{ errors.email }}</p>
        </div>

        <div>
            <label for="register-password" class="block text-sm font-medium text-slate-700"
                >Jelszó</label
            >
            <input
                id="register-password"
                v-model="password"
                type="password"
                autocomplete="new-password"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Legalább 8 karakter"
            />
            <p v-if="errors.password" class="text-sm text-red-600 mt-1">{{ errors.password }}</p>
        </div>

        <div>
            <label for="register-password-confirm" class="block text-sm font-medium text-slate-700"
                >Jelszó újra</label
            >
            <input
                id="register-password-confirm"
                v-model="passwordConfirm"
                type="password"
                autocomplete="new-password"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Ismételd meg a jelszót"
            />
            <p v-if="errors.passwordConfirm" class="text-sm text-red-600 mt-1">
                {{ errors.passwordConfirm }}
            </p>
        </div>

        <label class="inline-flex items-center gap-2 text-sm text-slate-700">
            <input
                v-model="acceptedTerms"
                type="checkbox"
                class="rounded border-slate-300 bg-white shadow-sm"
            />
            Elfogadom a feltételeket
        </label>
        <p v-if="errors.acceptedTerms" class="text-sm text-red-600">{{ errors.acceptedTerms }}</p>

        <button
            type="submit"
            class="w-full rounded-lg bg-slate-900 text-white py-2.5 font-medium hover:bg-slate-800 transition"
        >
            Regisztráció
        </button>
    </form>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'

export interface RegisterPayload {
    name: string
    email: string
    password: string
}

const emit = defineEmits<{
    submit: [payload: RegisterPayload]
}>()

const name = ref('')
const email = ref('')
const password = ref('')
const passwordConfirm = ref('')
const acceptedTerms = ref(false)

const errors = reactive({
    name: '',
    email: '',
    password: '',
    passwordConfirm: '',
    acceptedTerms: '',
})

function validate() {
    errors.name = ''
    errors.email = ''
    errors.password = ''
    errors.passwordConfirm = ''
    errors.acceptedTerms = ''

    if (!name.value) errors.name = 'A név kötelező.'
    if (!email.value) errors.email = 'Az e-mail cím kötelező.'
    if (password.value.length < 8) errors.password = 'A jelszó legalább 8 karakter legyen.'
    if (password.value !== passwordConfirm.value) {
        errors.passwordConfirm = 'A két jelszó nem egyezik.'
    }
    if (!acceptedTerms.value) errors.acceptedTerms = 'A regisztrációhoz fogadd el a feltételeket.'

    return !Object.values(errors).some(Boolean)
}

function onSubmit() {
    if (!validate()) return

    emit('submit', {
        name: name.value,
        email: email.value,
        password: password.value,
    })
}
</script>
