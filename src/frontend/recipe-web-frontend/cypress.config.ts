import { defineConfig } from 'cypress'

export default defineConfig({
    e2e: {
        baseUrl: 'https://localhost:9393',
        specPattern: 'cypress/e2e/**/*.cy.{ts,tsx}',
        supportFile: 'cypress/support/e2e.ts',
    },
})
