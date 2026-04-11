<template>
    <section class="bg-white/90 backdrop-blur rounded-2xl shadow-xl border border-slate-100 p-6 sm:p-8">
        <h1 class="text-2xl font-bold text-slate-900">{{ title }}</h1>
        <p v-if="subtitle" class="text-sm text-slate-600 mt-1">{{ subtitle }}</p>
        <div class="mt-6">
            <slot />
        </div>
    </section>
</template>

<script setup lang="ts">
defineProps<{
    title: string
    subtitle?: string
}>()
</script>
