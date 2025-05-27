import { createRouter, createWebHistory } from 'vue-router'
import MainSite from '../views/MainSite.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: MainSite,
        },
    ],
})

export default router
