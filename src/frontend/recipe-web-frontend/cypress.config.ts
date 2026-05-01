import { defineConfig } from 'cypress'

export default defineConfig({
    e2e: {
        baseUrl:
            process.env.APP_HOST && process.env.APP_PORT
                ? `${process.env.PROXY_TLS_ENABLED === 'true' ? 'https' : 'http'}://${process.env.APP_HOST}:${process.env.APP_PORT}`
                : 'http://localhost:9393',
        specPattern: 'cypress/e2e/**/*.cy.{ts,tsx}',
        supportFile: 'cypress/support/e2e.ts',
    },
})
