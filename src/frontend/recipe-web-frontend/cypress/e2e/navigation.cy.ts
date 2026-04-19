describe('Navigation', () => {
    beforeEach(() => {
        cy.visit('/')
    })

    context('Brand / logo', () => {
        it('shows the "ReceptApp" brand link', () => {
            cy.contains('nav a', 'ReceptApp').should('be.visible')
        })

        it('navigates to / when the brand link is clicked', () => {
            cy.visit('/recipes')
            cy.contains('nav a', 'ReceptApp').click()
            cy.url().should('eq', Cypress.config('baseUrl') + '/')
        })
    })

    context('Main navigation links', () => {
        it('has a "Receptek" link pointing to /recipes', () => {
            cy.contains('nav a', 'Receptek').should('have.attr', 'href', '/recipes')
        })

        it('navigates to /recipes when "Receptek" is clicked', () => {
            cy.contains('nav a', 'Receptek').click()
            cy.url().should('include', '/recipes')
        })

        it('has an "Új recept" link in the nav', () => {
            cy.contains('nav a', 'Új recept').should('exist')
        })
    })

    context('Guest state', () => {
        it('shows a "Belépés" button when not logged in', () => {
            cy.contains('nav button', 'Belépés').should('be.visible')
        })

        it('does not show a "Kilépés" button when not logged in', () => {
            cy.contains('nav button', 'Kilépés').should('not.exist')
        })

        it('navigates to /login when the "Belépés" button is clicked', () => {
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

    context('Authenticated state', () => {
        beforeEach(() => {
            cy.login()
        })

        it('shows the logged-in user display name in the nav', () => {
            cy.contains('nav', Cypress.env('userDisplayName')).should('be.visible')
        })

        it('shows a "Kilépés" button when logged in', () => {
            cy.contains('nav button', 'Kilépés').should('be.visible')
        })

        it('does not show a "Belépés" button when logged in', () => {
            cy.contains('nav button', 'Belépés').should('not.exist')
        })

        it('clicking "Kilépés" logs out and redirects to /login', () => {
            cy.contains('nav button', 'Kilépés').click()
            cy.url().should('include', '/login')
            cy.contains('nav button', 'Belépés').should('be.visible')
        })
    })
})
