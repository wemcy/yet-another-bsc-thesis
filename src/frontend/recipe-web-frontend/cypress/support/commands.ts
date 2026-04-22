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
        }
    }
}

Cypress.Commands.add(
    'login',
    (email = Cypress.env('userEmail'), password = Cypress.env('userPassword')) => {
        cy.visit('/login')
        cy.get('#login-email').type(email)
        cy.get('#login-password').type(password)
        cy.contains('button[type="submit"]', 'Bejelentkezés').click()
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

export {}
