describe('All Recipes Page', () => {
    context('Page structure', () => {
        beforeEach(() => {
            cy.visit('/recipes')
        })

        it('displays the "Receptek" page heading', () => {
            cy.contains('h1', 'Receptek').should('be.visible')
        })

        it('shows the allergen filter section', () => {
            cy.contains('h2', 'Allergén szűrő').should('be.visible')
        })

        it('has a title search input', () => {
            cy.get('#recipe-title-filter').should('be.visible')
        })

        it('has an "Apply filter" button that is initially disabled', () => {
            cy.contains('button', 'Szűrő alkalmazása').should('be.disabled')
        })

        it('has a "Clear filters" button that is initially disabled', () => {
            cy.contains('button', 'Szűrők törlése').should('be.disabled')
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
    })

    context('Filter behavior', () => {
        beforeEach(() => {
            cy.visit('/recipes')
        })

        it('enables the Apply button after typing in the title field', () => {
            cy.get('#recipe-title-filter').type('teszt')
            cy.contains('button', 'Szűrő alkalmazása').should('not.be.disabled')
        })

        it('enables the Clear button after typing in the title field', () => {
            cy.get('#recipe-title-filter').type('teszt')
            cy.contains('button', 'Szűrők törlése').should('not.be.disabled')
        })

        it('updates the URL with the title query param after applying the filter', () => {
            cy.get('#recipe-title-filter').type('gulyás')
            cy.contains('button', 'Szűrő alkalmazása').click()
            cy.url().should('include', 'title=guly%C3%A1s')
        })

        it('clears the title field and disables the Clear button after clicking "Szűrők törlése"', () => {
            cy.get('#recipe-title-filter').type('gulyás')
            cy.contains('button', 'Szűrők törlése').click()
            cy.get('#recipe-title-filter').should('have.value', '')
            cy.contains('button', 'Szűrők törlése').should('be.disabled')
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

        it('shows the empty-state message when the search returns no results', () => {
            // An intentionally nonsensical search string that should never match real recipes
            cy.get('#recipe-title-filter').type('xyzzyplughcypress999')
            cy.contains('button', 'Szűrő alkalmazása').click()
            cy.contains('Nincs találat a megadott keresésre és szűrőkre.').should('be.visible')
        })
    })
})
