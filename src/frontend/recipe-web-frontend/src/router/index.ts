import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import ProfileView from '@/views/ProfileView.vue'
import PublicProfileView from '@/views/PublicProfileView.vue'
import RecipeView from '@/views/RecipeView.vue'
import NotFoundView from '@/views/NotFoundView.vue'
import NewRecipeView from '@/views/NewRecipeView.vue'
import AllRecipesView from '@/views/AllRecipesView.vue'
import EditRecipeView from '@/views/EditRecipeView.vue'
import LoginView from '@/views/LoginView.vue'
import { useAuthStore } from '@/stores/authStore'

const initRouter = () => {
    const router = createRouter({
        history: createWebHistory(import.meta.env.BASE_URL),
        scrollBehavior(_to, _from, savedPosition) {
            if (savedPosition) return savedPosition
            return { top: 0 }
        },
        routes: [
            { path: '/', name: 'Home', component: HomeView },
            {
                path: '/profile',
                name: 'Profile',
                component: ProfileView,
                meta: { requiresAuth: true },
            },
            {
                path: '/profile/:id',
                name: 'PublicProfile',
                component: PublicProfileView,
            },
            { path: '/recipe/:id', name: 'Recipe', component: RecipeView },
            {
                path: '/new-recipe',
                name: 'NewRecipe',
                component: NewRecipeView,
                meta: { requiresAuth: true },
            },
            {
                path: '/recipes',
                name: 'AllRecipes',
                component: AllRecipesView,
            },
            {
                path: '/edit/:id',
                name: 'EditRecipe',
                component: EditRecipeView,
                props: true,
            },
            { path: '/login', name: 'Login', component: LoginView },
            { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFoundView },
        ],
    })

    router.beforeEach((to) => {
        const auth = useAuthStore()

        if (to.meta.requiresAuth && !auth.isLoggedIn) {
            return {
                name: 'Login',
                query: { redirect: to.fullPath },
            }
        }

        return true
    })

    return router
}

export default initRouter
