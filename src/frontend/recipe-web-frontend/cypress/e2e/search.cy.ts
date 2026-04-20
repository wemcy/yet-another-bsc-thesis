describe('Search Autocomplete', () => {
    beforeEach(() => {
        cy.visit('/')
    })

    it('shows the search input with the correct placeholder', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').should('be.visible')
    })

    it('does not show the dropdown when fewer than 2 characters are typed', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').type('a')
        // The dropdown should not appear
        cy.get('[class*="absolute"][class*="z-50"]').should('not.exist')
    })

    it('shows the dropdown after typing at least 2 characters', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').type('re')
        // Wait for debounced search
        cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
    })

    it('shows search results or "Nincs találat" message', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').type('re')
        cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
        cy.get('[class*="absolute"][class*="z-50"]').then(($dropdown) => {
            // Either we have result items or "Nincs találat"
            const text = $dropdown.text()
            expect(text.length).to.be.greaterThan(0)
        })
    })

    it('shows "Nincs találat." for a nonsensical search term', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').type('xyzzyplugh999')
        cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
        cy.contains('Nincs találat.').should('be.visible')
    })

    it('navigates to the recipe page when a result is selected', () => {
        cy.get('input[placeholder="Keresés receptek között..."]').type('re')
        cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
        // Wait until actual results appear (not loading)
        cy.get('[class*="absolute"][class*="z-50"]').then(($dropdown) => {
            if (
                !$dropdown.text().includes('Nincs találat') &&
                !$dropdown.text().includes('Keresés...')
            ) {
                cy.get('[class*="absolute"][class*="z-50"] [class*="cursor-pointer"]')
                    .first()
                    .click()
                cy.url().should('include', '/recipe/')
            }
        })
    })
})
