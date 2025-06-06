<script setup lang="ts">
import { ref } from 'vue'

const editing = ref(false)
const profile = ref({
    name: 'Kovács Anna',
    email: 'anna.kovacs@example.com',
    password: '',
    registered: '2023-11-01',
})

const original = ref({ ...profile.value })

function startEdit() {
    original.value = { ...profile.value }
    editing.value = true
}

function cancelEdit() {
    profile.value = { ...original.value }
    editing.value = false
}

function saveEdit() {
    // Itt jöhetne majd backend hívás
    editing.value = false
}
</script>

<template>
    <div class="max-w-xl mx-auto bg-white shadow p-6 rounded-md mt-10">
        <div class="flex justify-between items-start mb-6">
            <div>
                <label class="block font-semibold mb-1">Profil kép</label>
                <div
                    class="w-16 h-16 border rounded-full flex items-center justify-center text-gray-400"
                >
                    X
                </div>
            </div>
            <button
                v-if="!editing"
                @click="startEdit"
                class="border px-4 py-1 rounded hover:bg-gray-100"
            >
                Szerkesztés
            </button>
        </div>

        <div class="space-y-4">
            <div>
                <label class="block font-semibold">Név</label>
                <input
                    v-if="editing"
                    v-model="profile.name"
                    class="border rounded px-3 py-1 w-full"
                />
                <p v-else>{{ profile.name }}</p>
            </div>

            <div>
                <label class="block font-semibold">Email</label>
                <input
                    v-if="editing"
                    v-model="profile.email"
                    class="border rounded px-3 py-1 w-full"
                />
                <p v-else>{{ profile.email }}</p>
            </div>

            <div>
                <label class="block font-semibold">Jelszó</label>
                <input
                    v-if="editing"
                    v-model="profile.password"
                    type="password"
                    class="border rounded px-3 py-1 w-full"
                />
                <label v-if="editing" class="block font-semibold">Jelszó megerősítése</label>
                <input
                    v-if="editing"
                    v-model="profile.password"
                    type="password"
                    class="border rounded px-3 py-1 w-full"
                />
                <p v-else>********</p>
            </div>

            <div>
                <label class="block font-semibold">Regisztráció dátuma</label>
                <p>{{ profile.registered }}</p>
            </div>
        </div>

        <div v-if="editing" class="flex justify-end gap-2 mt-6">
            <button @click="cancelEdit" class="border px-4 py-1 rounded hover:bg-gray-100">
                Mégse
            </button>
            <button
                @click="saveEdit"
                class="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
            >
                Mentés
            </button>
        </div>
    </div>
</template>
