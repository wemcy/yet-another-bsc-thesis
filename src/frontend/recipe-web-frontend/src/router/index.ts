import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import ProfileView from '@/views/ProfileView.vue'
import RecipeView from '@/views/RecipeView.vue'
import NotFoundView from '@/views/NotFoundView.vue'
import NewRecipeView from '@/views/NewRecipeView.vue'
import AllRecipesView from '@/views/AllRecipesView.vue'
import EditRecipeView from '@/views/EditRecipeView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        { path: '/', name: 'Home', component: HomeView },
        { path: '/profile', name: 'Profile', component: ProfileView },
        { path: '/recipe/:id', name: 'Recipe', component: RecipeView },
        { path: '/new-recipe', name: 'NewRecipe', component: NewRecipeView },
        {
            path: '/recipes',
            name: 'AllRecipes',
            component: AllRecipesView,
        },
        {
            path: '/edit/:id',
            name: 'EditRecipe',
            component: EditRecipeView,
            props: true, // így átmegy az id propban
        },
        { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFoundView },
    ],
})

export default router
