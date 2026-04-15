<template>
    <section
        class="flex flex-col items-center gap-4 bg-white rounded-xl shadow p-6 w-full max-w-2xl mx-auto"
    >
        <ProfileAvatar :src="avatarUrl" :alt="displayName" size="lg" />
        <h2 class="text-xl font-semibold">{{ displayName }}</h2>
    </section>
</template>

<script setup lang="ts">
import ProfileAvatar from '@/components/profile/ProfileAvatar.vue'

defineProps<{
    displayName: string
    avatarUrl?: string
}>()
</script>
