import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import initRouter from './router'
import { vueKeycloak } from '@josempgon/vue-keycloak'

const app = createApp(App)

await vueKeycloak.install(app, {
    config: {
        url: 'http://localhost:9393/auth',
        realm: 'recipeapp',
        clientId: 'recipeapp-frontend',
    },
    initOptions: {
        onLoad: 'check-sso',
        checkLoginIframe: true,
    },
})
app.use(initRouter())
app.use(createPinia())

app.mount('#app')
