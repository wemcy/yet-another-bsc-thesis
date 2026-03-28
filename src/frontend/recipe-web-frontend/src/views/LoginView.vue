<template>
    <main class="min-h-[80vh] px-4 py-12">
        <div class="max-w-md mx-auto">
            <div
                class="mb-6 grid grid-cols-2 bg-white rounded-xl p-1 shadow border border-slate-100"
            >
                <button
                    type="button"
                    @click="mode = 'login'"
                    :class="[
                        'rounded-lg py-2 text-sm font-medium transition',
                        mode === 'login'
                            ? 'bg-blue-600 text-white shadow'
                            : 'text-slate-600 hover:bg-slate-100',
                    ]"
                >
                    Bejelentkezés
                </button>
                <button
                    type="button"
                    @click="mode = 'register'"
                    :class="[
                        'rounded-lg py-2 text-sm font-medium transition',
                        mode === 'register'
                            ? 'bg-slate-900 text-white shadow'
                            : 'text-slate-600 hover:bg-slate-100',
                    ]"
                >
                    Regisztráció
                </button>
            </div>

            <FormCard
                :title="mode === 'login' ? 'Szia, újra itt!' : 'Készíts új fiókot'"
                :subtitle="
                    mode === 'login'
                        ? 'Adj meg e-mailt és jelszót a belépéshez.'
                        : 'Pár adat, és már kész is a regisztrációs űrlap.'
                "
            >
                <LoginForm v-if="mode === 'login'" @submit="handleLogin" />
                <RegisterForm v-else @submit="handleRegister" />

                <p
                    v-if="feedback"
                    class="mt-4 text-sm rounded-lg border border-emerald-200 bg-emerald-50 text-emerald-700 px-3 py-2"
                >
                    {{ feedback }}
                </p>
                <p
                    v-if="auth.authError"
                    class="mt-4 text-sm rounded-lg border border-red-200 bg-red-50 text-red-700 px-3 py-2"
                >
                    {{ auth.authError }}
                </p>
                <p v-if="auth.authLoading" class="mt-3 text-xs text-slate-500">Feldolgozas...</p>
            </FormCard>
        </div>
    </main>
</template>

<script setup lang="ts">
import FormCard from '@/components/common/FormCard.vue'
import LoginForm, { type LoginPayload } from '@/components/auth/LoginForm.vue'
import RegisterForm, { type RegisterPayload } from '@/components/auth/RegisterForm.vue'
import { useAuthStore } from '@/stores/authStore'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

type AuthMode = 'login' | 'register'

const mode = ref<AuthMode>('login')
const feedback = ref('')
const auth = useAuthStore()
const router = useRouter()

async function handleLogin(payload: LoginPayload) {
    feedback.value = ''

    try {
        await auth.loginWithCredentials(payload.email, payload.password)
        feedback.value = `Sikeres bejelentkezes: ${payload.email}`
        await router.push('/profile')
    } catch {
        // Error details are normalized in authStore.authError
    }
}

async function handleRegister(payload: RegisterPayload) {
    feedback.value = ''

    try {
        await auth.registerWithCredentials(payload.name, payload.email, payload.password)
        feedback.value = `Sikeres regisztracio: ${payload.email}`
        await router.push('/profile')
    } catch {
        // Error details are normalized in authStore.authError
    }
}
</script>
