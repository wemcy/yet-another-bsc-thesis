<template>
    <form class="space-y-4" @submit.prevent="onSubmit">
        <div>
            <label for="login-email" class="block text-sm font-medium text-slate-700">E-mail</label>
            <input
                id="login-email"
                v-model.trim="email"
                type="email"
                autocomplete="email"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="te@domain.hu"
            />
            <p v-if="errors.email" class="text-sm text-red-600 mt-1">{{ errors.email }}</p>
        </div>

        <div>
            <label for="login-password" class="block text-sm font-medium text-slate-700"
                >Jelszó</label
            >
            <input
                id="login-password"
                v-model="password"
                type="password"
                autocomplete="current-password"
                class="mt-1 w-full rounded-lg border border-slate-300 bg-white shadow-sm px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="••••••••"
            />
            <p v-if="errors.password" class="text-sm text-red-600 mt-1">{{ errors.password }}</p>
        </div>

        <label class="inline-flex items-center gap-2 text-sm text-slate-700">
            <input
                v-model="remember"
                type="checkbox"
                class="rounded border-slate-300 bg-white shadow-sm"
            />
            Emlékezz rám
        </label>

        <button
            type="submit"
            class="w-full rounded-lg bg-blue-600 text-white py-2.5 font-medium hover:bg-blue-700 transition"
        >
            Bejelentkezés
        </button>
    </form>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'

export interface LoginPayload {
    email: string
    password: string
    remember: boolean
}

const emit = defineEmits<{
    submit: [payload: LoginPayload]
}>()

const email = ref('')
const password = ref('')
const remember = ref(false)

const errors = reactive({
    email: '',
    password: '',
})

function validate() {
    errors.email = ''
    errors.password = ''

    if (!email.value) {
        errors.email = 'Az e-mail cím kötelező.'
    }

    if (!password.value) {
        errors.password = 'A jelszó kötelező.'
    }

    return !errors.email && !errors.password
}

function onSubmit() {
    if (!validate()) return

    emit('submit', {
        email: email.value,
        password: password.value,
        remember: remember.value,
    })
}
</script>
