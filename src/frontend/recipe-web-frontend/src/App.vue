<script setup lang="ts">
import { RouterView } from 'vue-router'
import HeaderNav from './components/HeaderNav.vue'
import MainFooter from './components/MainFooter.vue'
</script>

<template>
    <HeaderNav></HeaderNav>
    <div>
        <router-view />
    </div>
    <MainFooter></MainFooter>
</template>

<style scoped></style>
