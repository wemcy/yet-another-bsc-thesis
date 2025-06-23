<template>
    <img
        :src="errored ? defaultAvatar : src || defaultAvatar"
        :alt="alt"
        :class="['rounded-full border-2 border-blue-400 object-cover', sizeClass]"
        @error="onError"
    />
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import type { AvatarProps } from '@/types/profile/avatar'
import defaultAvatar from '@/assets/profile.svg'

const props = defineProps<AvatarProps>()

const errored = ref(false)
const onError = () => {
    errored.value = true
}

const sizeClass = computed(() => {
    switch (props.size) {
        case 'sm':
            return 'w-8 h-8'
        case 'lg':
            return 'w-16 h-16'
        default:
            return 'w-10 h-10'
    }
})
</script>
