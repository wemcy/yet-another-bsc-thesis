/// <reference types="cypress" />

declare global {
    namespace Cypress {
        interface Chainable {
            /**
             * Log in via the real backend.
             * Credentials default to the `userEmail` / `userPassword` Cypress env vars.
             * Waits until the browser has navigated away from /login.
             */
            login(email?: string, password?: string): Chainable<void>
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

export {}
