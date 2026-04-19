describe('404 – Not Found Page', () => {
    beforeEach(() => {
        cy.visit('/this-page-does-not-exist', { failOnStatusCode: false })
    })

    it('displays the "404" heading', () => {
        cy.contains('h1', '404').should('be.visible')
    })

    it('displays the "page not found" message', () => {
        cy.contains('A keresett oldal nem található.').should('be.visible')
    })

    it('has a link back to the home page', () => {
        cy.contains('a', 'Vissza a főoldalra').should('be.visible').and('have.attr', 'href', '/')
    })

    it('navigates back to the home page when the link is clicked', () => {
        cy.contains('a', 'Vissza a főoldalra').click()
        cy.url().should('eq', Cypress.config('baseUrl') + '/')
    })
})
