describe('Navigation [support]', () => {
    beforeEach(() => {
        cy.visit('/')
    })

    context('Support - brand / logo', () => {
        it('shows the "ReceptApp" brand link', () => {
            cy.contains('nav a', 'ReceptApp').should('be.visible')
        })

        it('navigates to / when the brand link is clicked', () => {
            cy.visit('/recipes')
            cy.contains('nav a', 'ReceptApp').click()
            cy.url().should('eq', Cypress.config('baseUrl') + '/')
        })
    })

    context('Support - main navigation links', () => {
        it('has a "Recipes" link pointing to /recipes', () => {
            cy.contains('nav a', 'Receptek').should('have.attr', 'href', '/recipes')
        })

        it('navigates to /recipes when "Receptek" is clicked', () => {
            cy.contains('nav a', 'Receptek').click()
            cy.url().should('include', '/recipes')
        })

        it('has a "New recipe" link in the nav', () => {
            cy.contains('nav a', 'Új recept').should('exist')
        })
    })

    context('Support - guest state', () => {
        it('shows a "Login" button when not logged in', () => {
            cy.contains('nav button', 'Belépés').should('be.visible')
        })

        it('does not show a "Logout" button when not logged in', () => {
            cy.contains('nav button', 'Kilépés').should('not.exist')
        })

        it('navigates to /login when the "Login" button is clicked', () => {
            cy.contains('nav button', 'Belépés').click()
            cy.url().should('include', '/login')
        })

        it('visiting /new-recipe redirects to /login (auth guard)', () => {
            cy.visit('/new-recipe')
            cy.url().should('include', '/login')
        })

        it('visiting /profile redirects to /login (auth guard)', () => {
            cy.visit('/profile')
            cy.url().should('include', '/login')
        })
    })

    context('Support - authenticated state', () => {
        beforeEach(() => {
            cy.login()
        })

        it('shows the logged-in user display name in the nav', () => {
            cy.contains('nav', Cypress.env('userDisplayName')).should('be.visible')
        })

        it('shows a "Logout" button when logged in', () => {
            cy.contains('nav button', 'Kilépés').should('be.visible')
        })

        it('does not show a "Login" button when logged in', () => {
            cy.contains('nav button', 'Belépés').should('not.exist')
        })

        it('clicking "Logout" logs out and redirects to /login', () => {
            cy.contains('nav button', 'Kilépés').click()
            cy.url().should('include', '/login')
            cy.contains('nav button', 'Belépés').should('be.visible')
        })
    })
})
