describe('All Recipes Page [U03A, U03B]', () => {
    context('U03A - Recipe search with results', () => {
        beforeEach(() => {
            cy.visit('/recipes')
        })

        it('displays the "Recipes" page heading', () => {
            cy.contains('h1', 'Receptek').should('be.visible')
        })

        it('shows the allergen filter section', () => {
            cy.contains('h2', 'Allergén szűrő').should('be.visible')
        })

        it('has a title search input', () => {
            cy.get('[data-cy="recipe-title-filter"]').should('be.visible')
        })

        it('has an "Apply filter" button that is initially disabled', () => {
            cy.get('[data-cy="apply-filters"]').should('be.disabled')
        })

        it('has a "Clear filters" button that is initially disabled', () => {
            cy.get('[data-cy="clear-filters"]').should('be.disabled')
        })

        it('displays allergen checkboxes in the "must include" section', () => {
            cy.contains('h3', 'Kötelezően tartalmazza').should('be.visible')
            cy.contains('label', 'Glutén').should('exist')
            cy.contains('label', 'Tojás').should('exist')
            cy.contains('label', 'Tej').should('exist')
        })

        it('displays allergen checkboxes in the "must exclude" section', () => {
            cy.contains('h3', 'Nem tartalmazhatja').should('be.visible')
        })

        it('shows recipe cards from the backend', () => {
            cy.get('a[href*="/recipe/"]').should('have.length.at.least', 1)
        })

        it('enables the Apply button after typing in the title field', () => {
            cy.get('[data-cy="recipe-title-filter"]').type('teszt')
            cy.get('[data-cy="apply-filters"]').should('not.be.disabled')
        })

        it('enables the Clear button after typing in the title field', () => {
            cy.get('[data-cy="recipe-title-filter"]').type('teszt')
            cy.get('[data-cy="clear-filters"]').should('not.be.disabled')
        })

        it('updates the URL with the title query param after applying the filter', () => {
            cy.get('[data-cy="recipe-title-filter"]').type('gulyás')
            cy.get('[data-cy="apply-filters"]').click()
            cy.url().should('include', 'title=guly%C3%A1s')
        })

        it('clears the title field and disables the Clear button after clicking "Clear filters"', () => {
            cy.get('[data-cy="recipe-title-filter"]').type('gulyás')
            cy.get('[data-cy="clear-filters"]').click()
            cy.get('[data-cy="recipe-title-filter"]').should('have.value', '')
            cy.get('[data-cy="clear-filters"]').should('be.disabled')
        })

        it('enables the Apply button after toggling an include-allergen checkbox', () => {
            cy.contains('h3', 'Kötelezően tartalmazza')
                .parent()
                .contains('label', 'Glutén')
                .find('input[type="checkbox"]')
                .check()
            cy.contains('button', 'Szűrő alkalmazása').should('not.be.disabled')
        })

        it('enables the Apply button after toggling an exclude-allergen checkbox', () => {
            cy.contains('h3', 'Nem tartalmazhatja')
                .parent()
                .contains('label', 'Tojás')
                .find('input[type="checkbox"]')
                .check()
            cy.contains('button', 'Szűrő alkalmazása').should('not.be.disabled')
        })

        it('a checked include-allergen cannot simultaneously be checked as exclude', () => {
            const getGlutenInclude = () =>
                cy
                    .contains('h3', 'Kötelezően tartalmazza')
                    .parent()
                    .contains('label', 'Glutén')
                    .find('input[type="checkbox"]')
            const getGlutenExclude = () =>
                cy
                    .contains('h3', 'Nem tartalmazhatja')
                    .parent()
                    .contains('label', 'Glutén')
                    .find('input[type="checkbox"]')

            getGlutenInclude().check()
            getGlutenExclude().check()

            getGlutenInclude().should('not.be.checked')
        })
    })

    context('U03B - Recipe search without results', () => {
        beforeEach(() => {
            cy.visit('/recipes')
        })

        it('shows the empty-state message when the search returns no results', () => {
            // An intentionally nonsensical search string that should never match real recipes
            cy.get('[data-cy="recipe-title-filter"]').type('xyzzyplughcypress999')
            cy.get('[data-cy="apply-filters"]').click()
            cy.contains('Nincs találat a megadott keresésre és szűrőkre.').should('be.visible')
        })
    })

    context('Support - pagination', () => {
        beforeEach(() => {
            cy.visit('/recipes')
        })

        it('shows the pagination controls when there are enough recipes', () => {
            cy.get('body').then(($body) => {
                if ($body.find('button:contains("Következő")').length > 0) {
                    cy.contains('button', 'Következő').should('be.visible')
                    cy.contains('button', 'Előző').should('be.visible')
                } else {
                    cy.log('Not enough recipes for pagination — skipping')
                }
            })
        })

        it('disables the Previous button on the first page', () => {
            cy.get('body').then(($body) => {
                if ($body.find('button:contains("Előző")').length > 0) {
                    cy.contains('button', 'Előző').should('be.disabled')
                } else {
                    cy.log('No pagination present — skipping')
                }
            })
        })

        it('navigates to the next page and updates the URL', () => {
            cy.get('body').then(($body) => {
                if (
                    $body.find('button:contains("Következő")').length > 0 &&
                    !$body.find('button:contains("Következő")').is(':disabled')
                ) {
                    cy.contains('button', 'Következő').click()
                    cy.url().should('include', 'page=2')
                    cy.get('a[href*="/recipe/"]').should('have.length.at.least', 1)
                } else {
                    cy.log('No next page available — skipping')
                }
            })
        })

        it('highlights the current page number', () => {
            cy.get('body').then(($body) => {
                if ($body.find('button:contains("Következő")').length > 0) {
                    // Page 1 button should have the active styling (bg-blue-600)
                    cy.get('button.bg-blue-600').should('contain.text', '1')
                } else {
                    cy.log('No pagination present — skipping')
                }
            })
        })
    })
})
