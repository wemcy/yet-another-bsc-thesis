describe('Home Page', () => {
    beforeEach(() => {
        cy.visit('/')
    })

    context('Hero section', () => {
        it('displays the main heading', () => {
            cy.contains('h1', 'Fedezd fel a legjobb házias recepteket').should('be.visible')
        })

        it('displays the subtitle with allergén mention', () => {
            cy.contains('allergénszűrővel').should('be.visible')
        })

        it('has a CTA button linking to /recipes', () => {
            cy.contains('a', 'Böngészés a receptek között')
                .should('be.visible')
                .and('have.attr', 'href', '/recipes')
        })
    })

    context('Showcase section', () => {
        it('displays the "Kiemelt receptek" heading', () => {
            cy.contains('h2', 'Kiemelt receptek').should('be.visible')
        })

        it('shows recipe cards after data loads', () => {
            cy.get('a[href*="/recipe/"]').should('have.length.at.least', 1)
        })
    })

    context('Featured recipe', () => {
        it('renders the featured recipe section', () => {
            // The featured recipe block renders once the API responds
            cy.get('main section, main article, main [class*="highlight"]').should('exist')
        })
    })

    context('Navigation for guests', () => {
        it('shows the login button and not the logout button', () => {
            cy.contains('nav button', 'Belépés').should('be.visible')
            cy.contains('nav button', 'Kilépés').should('not.exist')
        })
    })
})
