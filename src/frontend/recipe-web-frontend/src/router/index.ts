import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import ProfileView from '@/views/ProfileView.vue'
import RecipeView from '@/views/RecipeView.vue'
import NotFoundView from '@/views/NotFoundView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        { path: '/', name: 'Home', component: HomeView },
        { path: '/profile', name: 'Profile', component: ProfileView },
        { path: '/recipe/:id', name: 'Recipe', component: RecipeView },
        { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFoundView }, // fallback!
    ],
})

export default router
