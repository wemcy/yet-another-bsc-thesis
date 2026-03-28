<script setup lang="ts">
import { RouterView } from 'vue-router'
import HeaderNav from './components/HeaderNav.vue'
import MainFooter from './components/MainFooter.vue'
</script>

<template>
    <div class="min-h-screen bg-gradient-to-br from-sky-100 via-white to-cyan-100 flex flex-col">
        <HeaderNav></HeaderNav>
        <div class="flex-1">
            <router-view />
        </div>
        <MainFooter></MainFooter>
    </div>
</template>

<style scoped></style>
