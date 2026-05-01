/// <reference types="cypress" />

declare global {
    // eslint-disable-next-line @typescript-eslint/no-namespace
    namespace Cypress {
        interface Chainable {
            /**
             * Log in through the UI login form.
             * Defaults to the admin credentials from Cypress env vars.
             * Uses the full UI flow so the app's Pinia store receives the
             * complete login response (including roles).
             */
            login(email?: string, password?: string): Chainable<void>

            /**
             * Ensure the non-admin test user exists (registers via API if needed),
             * then log in as that user through the UI.
             */
            loginAsTestUser(): Chainable<void>
            seedRecipes(): Chainable<void>
            cleanupSeededRecipes(): Chainable<void>
        }
    }
}

Cypress.Commands.add(
    'login',
    (email = Cypress.env('userEmail'), password = Cypress.env('userPassword')) => {
        cy.visit('/login')

        // Support both the new `data-cy` attributes and the legacy `id` selectors.
        // This makes the helper resilient if the running app hasn't been rebuilt yet.
        const emailSelector = '[data-cy="login-email"], #login-email'
        const passwordSelector = '[data-cy="login-password"], #login-password'
        const submitSelector = '[data-cy="login-submit"], button[type="submit"]'

        cy.get(emailSelector, { timeout: 10000 }).should('exist').type(email)
        cy.get(passwordSelector).type(password)
        cy.get(submitSelector).click()
        cy.url().should('not.include', '/login')
    },
)

Cypress.Commands.add('loginAsTestUser', () => {
    const email = Cypress.env('testUserEmail')
    const password = Cypress.env('testUserPassword')
    const displayName = Cypress.env('testUserDisplayName')

    // Register the test user; ignore 4xx responses (user already exists)
    cy.request({
        method: 'POST',
        url: '/api/auth/register',
        body: { email, password, displayName },
        failOnStatusCode: false,
    })

    cy.login(email, password)
})


let seededRecipeIds: string[] = []

Cypress.Commands.add('seedRecipes', () => {
    seededRecipeIds = []
    // Use the correct ingredient object format
    const recipes = Array.from({ length: 6 }).map((_, i) => ({
        title: `Seeded Recipe ${i + 1}`,
        description: `Description for seeded recipe ${i + 1}`,
        ingredients: [
            {
                name: `Ingredient ${i + 1}a`,
                quantity: 1 + i,
                unitOfMeasurement: 'g',
                allergens: [],
            },
            {
                name: `Ingredient ${i + 1}b`,
                quantity: 2 + i,
                unitOfMeasurement: 'ml',
                allergens: [],
            },
        ],
        steps: [`Step 1 for recipe ${i + 1}`, `Step 2 for recipe ${i + 1}`],
    }))
    cy.wrap(recipes).each((recipe) => {
        cy.request({
            method: 'POST',
            url: '/api/recipes',
            body: recipe,
        }).then((resp) => {
            if (resp.body && resp.body.id) {
                seededRecipeIds.push(resp.body.id)
            }
        })
    })
})

Cypress.Commands.add('cleanupSeededRecipes', () => {
    if (!seededRecipeIds.length) return
    cy.wrap(seededRecipeIds).each((id) => {
        cy.request({
            method: 'DELETE',
            url: `/api/recipes/${id}`,
            failOnStatusCode: false,
        })
    })
    seededRecipeIds = []
})

export {}
