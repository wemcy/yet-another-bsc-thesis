describe('New Recipe Page', () => {
    context('Auth guard', () => {
        it('redirects an unauthenticated user to /login', () => {
            cy.visit('/new-recipe')
            cy.url().should('include', '/login')
        })

        it('includes the redirect query param so the user returns after login', () => {
            cy.visit('/new-recipe')
            cy.url().should('include', 'redirect=')
            cy.url().should('include', 'new-recipe')
        })
    })

    context('Recipe form for authenticated users', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/new-recipe')
        })

        it('displays the "Új recept hozzáadása" heading', () => {
            cy.contains('h1', 'Új recept hozzáadása').should('be.visible')
        })

        it('has a "Recept neve" input field', () => {
            cy.contains('label', 'Recept neve').should('be.visible')
        })

        it('has a "Leírás" textarea', () => {
            cy.contains('label', 'Leírás').should('be.visible')
        })

        it('has an "Hozzávalók" section with an add button', () => {
            cy.contains('label', 'Hozzávalók').should('be.visible')
            cy.contains('button', '+ Hozzávaló hozzáadása').should('be.visible')
        })

        it('has an "Elkészítési lépések" section with an add button', () => {
            cy.contains('label', 'Elkészítési lépések').should('be.visible')
            cy.contains('button', '+ Lépés hozzáadása').should('be.visible')
        })

        it('has an "Allergének" section with checkboxes', () => {
            cy.contains('label', 'Allergének').should('be.visible')
            cy.contains('label', 'Glutén').should('exist')
        })

        it('has a file upload input for the recipe image', () => {
            cy.contains('label', 'Kép feltöltése').should('be.visible')
            cy.get('input[type="file"][accept*="image"]').should('exist')
        })

        it('shows a validation error when submitting with an empty title', () => {
            cy.contains('button[type="submit"]', 'Mentés').click()
            cy.contains('p', 'A cím megadása kötelező.').should('be.visible')
        })

        it('adds a new ingredient row when "+ Hozzávaló hozzáadása" is clicked', () => {
            cy.contains('button', '+ Hozzávaló hozzáadása').click()
            // There should now be at least 2 ingredient input groups
            cy.get('input[placeholder="Hozzávaló"]').should('have.length.at.least', 2)
        })

        it('adds a new step row when "+ Lépés hozzáadása" is clicked', () => {
            cy.contains('button', '+ Lépés hozzáadása').click()
            cy.get('textarea').should('have.length.at.least', 2)
        })

        it('removes an ingredient row when the ✕ button is clicked', () => {
            // Add one so we have at least 2
            cy.contains('button', '+ Hozzávaló hozzáadása').click()
            cy.get('input[placeholder="Hozzávaló"]').should('have.length.at.least', 2)
            // Click last ✕ in the ingredients section
            cy.contains('label', 'Hozzávalók').parent().find('button').contains('✕').last().click()
            cy.get('input[placeholder="Hozzávaló"]').should('have.length', 1)
        })

        it('removes a step row when the ✕ button is clicked', () => {
            cy.contains('button', '+ Lépés hozzáadása').click()
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('textarea')
                .should('have.length.at.least', 2)
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('button')
                .contains('✕')
                .last()
                .click()
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('textarea')
                .should('have.length', 1)
        })

        it('shows validation errors for missing description', () => {
            cy.contains('label', 'Recept neve').parent().find('input').type('Teszt recept')
            // Leave description empty, submit
            cy.contains('button[type="submit"]', 'Mentés').click()
            cy.contains('A leírás nem lehet üres.').should('be.visible')
        })
    })
})
