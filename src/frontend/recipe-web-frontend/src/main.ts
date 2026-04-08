import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import initRouter from './router'

const app = createApp(App)

app.use(createPinia())
app.use(initRouter())

app.mount('#app')
