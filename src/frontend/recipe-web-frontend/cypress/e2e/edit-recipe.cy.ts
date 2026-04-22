describe('Edit Recipe Page', () => {
    context('Navigation to edit page', () => {
        it('navigates to the edit page from a recipe the user owns', () => {
            cy.login()
            // Navigate to own recipe via profile
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
            cy.contains('a', 'Szerkesztés').click()
            cy.url().should('include', '/edit/')
        })
    })

    context('Edit form structure', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
            cy.contains('a', 'Szerkesztés').click()
            cy.url().should('include', '/edit/')
        })

        it('displays the "Recept szerkesztése" heading', () => {
            cy.contains('h1', 'Recept szerkesztése').should('be.visible')
        })

        it('has a pre-populated "Recept neve" input', () => {
            cy.contains('label', 'Recept neve')
                .parent()
                .find('input')
                .invoke('val')
                .should('not.be.empty')
        })

        it('has a pre-populated "Leírás" textarea', () => {
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
            cy.get('input[placeholder="Hozzávaló"]').first().invoke('val').should('not.be.empty')
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

        it('has a Mentés (save) submit button', () => {
            cy.contains('button[type="submit"]', 'Mentés').should('be.visible')
        })

        it('has a Mégse (cancel) button', () => {
            cy.contains('button', 'Mégse').should('be.visible')
        })
    })

    context('Edit form interactions', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
            cy.contains('a', 'Szerkesztés').click()
            cy.url().should('include', '/edit/')
        })

        it('adds a new ingredient row when "+ Hozzávaló hozzáadása" is clicked', () => {
            cy.get('input[placeholder="Hozzávaló"]').then(($before) => {
                const countBefore = $before.length
                cy.contains('button', '+ Hozzávaló hozzáadása').click()
                cy.get('input[placeholder="Hozzávaló"]').should('have.length', countBefore + 1)
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
            cy.get('input[placeholder="Hozzávaló"]').then(($before) => {
                const countBefore = $before.length
                // Click the last remove button
                cy.contains('label', 'Hozzávalók')
                    .parent()
                    .find('button')
                    .contains('✕')
                    .last()
                    .click()
                cy.get('input[placeholder="Hozzávaló"]').should('have.length', countBefore - 1)
            })
        })

        it('adds a new step row when "+ Lépés hozzáadása" is clicked', () => {
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

        it('navigates back to the recipe when Mégse is clicked', () => {
            cy.contains('button', 'Mégse').click()
            cy.url().should('include', '/recipe/')
            cy.url().should('not.include', '/edit/')
        })
    })

    context('Edit form validation', () => {
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
