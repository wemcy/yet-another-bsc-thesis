describe('Edit Recipe Page [U06A, U06B]', () => {
    context('U06A - Edit recipe with valid data', () => {
        beforeEach(() => {
            cy.login()
            cy.seedRecipes()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
            cy.contains('a', 'Szerkesztés').click()
            cy.url().should('include', '/edit/')
        })
        afterEach(() => {
            cy.cleanupSeededRecipes()
        })

        it('navigates to the edit page from a recipe the user owns', () => {
            cy.url().should('include', '/edit/')
        })

        it('displays the "Edit Recipe" heading', () => {
            cy.contains('h1', 'Recept szerkesztése').should('be.visible')
        })

        it('has a pre-populated "Recipe name" input', () => {
            cy.contains('label', 'Recept neve')
                .parent()
                .find('input')
                .invoke('val')
                .should('not.be.empty')
        })

        it('has a pre-populated "Description" textarea', () => {
            cy.contains('label', 'Leírás')
                .parent()
                .find('textarea')
                .invoke('val')
                .should('not.be.empty')
        })

        it('displays the current rating (read-only)', () => {
            cy.contains('label', 'Értékelés').should('be.visible')
        })

        it('has at least one ingredient row pre-filled', () => {
            cy.get('[data-cy="ingredient-name"]').first().invoke('val').should('not.be.empty')
        })

        it('has at least one step row pre-filled', () => {
            // Steps are in textareas within the steps section
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('textarea')
                .first()
                .invoke('val')
                .should('not.be.empty')
        })

        it('shows allergen dropdowns on ingredient rows', () => {
            cy.contains('label', 'Hozzávalók')
                .parent()
                .find('summary')
                .should('have.length.at.least', 1)
                .each(($summary) => {
                    cy.wrap($summary).should('contain.text', 'Allergének')
                })
        })

        it('has an image upload input', () => {
            cy.get('input[type="file"][accept*="image"]').should('exist')
        })

        it('shows the current recipe image preview', () => {
            cy.get('img[alt="Preview"]').should('exist')
        })

        it('has a Save submit button', () => {
            cy.contains('button[type="submit"]', 'Mentés').should('be.visible')
        })

        it('has a Cancel button', () => {
            cy.contains('button', 'Mégse').should('be.visible')
        })

        it('adds a new ingredient row when "+ Add ingredient" is clicked', () => {
            cy.get('[data-cy="ingredient-name"]').then(($before) => {
                const countBefore = $before.length
                cy.contains('button', '+ Hozzávaló hozzáadása').click()
                cy.get('[data-cy="ingredient-name"]').should('have.length', countBefore + 1)
                cy.contains('label', 'Hozzávalók')
                    .parent()
                    .find('summary')
                    .should('have.length', countBefore + 1)
                    .each(($summary) => {
                        cy.wrap($summary).should('contain.text', 'Allergének')
                    })
            })
        })

        it('removes an ingredient row when the ✕ button is clicked', () => {
            // First add one so we have at least 2
            cy.contains('button', '+ Hozzávaló hozzáadása').click()
            cy.get('[data-cy="ingredient-name"]').then(($before) => {
                const countBefore = $before.length
                // Click the last remove button
                cy.contains('label', 'Hozzávalók')
                    .parent()
                    .find('button')
                    .contains('✕')
                    .last()
                    .click()
                cy.get('[data-cy="ingredient-name"]').should('have.length', countBefore - 1)
            })
        })

        it('adds a new step row when "+ Add step" is clicked', () => {
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('textarea')
                .then(($before) => {
                    const countBefore = $before.length
                    cy.contains('button', '+ Lépés hozzáadása').click()
                    cy.contains('label', 'Elkészítési lépések')
                        .parent()
                        .find('textarea')
                        .should('have.length', countBefore + 1)
                })
        })

        it('removes a step row when the ✕ button is clicked', () => {
            cy.contains('button', '+ Lépés hozzáadása').click()
            cy.contains('label', 'Elkészítési lépések')
                .parent()
                .find('textarea')
                .then(($before) => {
                    const countBefore = $before.length
                    cy.contains('label', 'Elkészítési lépések')
                        .parent()
                        .find('button')
                        .contains('✕')
                        .last()
                        .click()
                    cy.contains('label', 'Elkészítési lépések')
                        .parent()
                        .find('textarea')
                        .should('have.length', countBefore - 1)
                })
        })

        it('navigates back to the recipe when Cancel is clicked', () => {
            cy.contains('button', 'Mégse').click()
            cy.url().should('include', '/recipe/')
            cy.url().should('not.include', '/edit/')
        })
    })

    context('U06B - Edit recipe with invalid data', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
            cy.contains('a', 'Szerkesztés').click()
            cy.url().should('include', '/edit/')
        })

        it('shows validation error when the title is cleared and form is submitted', () => {
            cy.contains('label', 'Recept neve').parent().find('input').clear()
            cy.contains('button[type="submit"]', 'Mentés').click()
            cy.contains('A cím megadása kötelező.').should('be.visible')
        })
    })
})
