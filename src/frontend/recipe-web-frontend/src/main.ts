import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { vueKeycloak } from '@josempgon/vue-keycloak'

const app = createApp(App)

app.use(vueKeycloak, {
    config: {
        url: 'http://localhost:9393/auth', //TODO make this configurable
        realm: 'recipeapp',
        clientId: 'recipeapp-frontend',
    },
})
app.use(router)
app.use(createPinia())

app.mount('#app')
