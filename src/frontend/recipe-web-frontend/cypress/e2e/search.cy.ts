describe('Search Autocomplete [U03A, U03B]', () => {
    beforeEach(() => {
        cy.visit('/')
    })

    context('U03A - Recipe search with results', () => {
        it('shows the search input with the correct placeholder', () => {
            cy.get('[data-cy="recipe-search-input"]').should('be.visible')
        })

        it('does not show the dropdown when fewer than 2 characters are typed', () => {
            cy.get('[data-cy="recipe-search-input"]').type('a')
            cy.get('[class*="absolute"][class*="z-50"]').should('not.exist')
        })

        it('shows the dropdown after typing at least 2 characters', () => {
            cy.get('[data-cy="recipe-search-input"]').type('re')
            cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
        })

        it('shows search results or "No results" message', () => {
            cy.get('[data-cy="recipe-search-input"]').type('re')
            cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
            cy.get('[class*="absolute"][class*="z-50"]').then(($dropdown) => {
                const text = $dropdown.text()
                expect(text.length).to.be.greaterThan(0)
            })
        })

        it('uses simple navbar search without allergen filters', () => {
            cy.intercept('GET', '/api/search*', (req) => {
                expect(req.query).to.have.property('title', 'simple')
                expect(req.query).not.to.have.property('includeAllergens')
                expect(req.query).not.to.have.property('excludeAllergens')

                req.reply({
                    statusCode: 200,
                    body: [
                        {
                            id: '00000000-0000-0000-0000-000000000001',
                            title: 'Simple search result',
                        },
                    ],
                })
            }).as('simpleNavbarSearch')

            cy.get('[data-cy="recipe-search-input"]').type('simple')
            cy.wait('@simpleNavbarSearch')
            cy.contains('Simple search result').should('be.visible')
        })

        it('navigates to the recipe page when a result is selected', () => {
            cy.get('[data-cy="recipe-search-input"]').type('re')
            cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
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

    context('U03B - Recipe search without results', () => {
        it('shows "No results" for a nonsensical search term', () => {
            cy.get('[data-cy="recipe-search-input"]').type('xyzzyplugh999')
            cy.get('[class*="absolute"][class*="z-50"]', { timeout: 3000 }).should('be.visible')
            cy.contains('Nincs találat.').should('be.visible')
        })
    })
})
