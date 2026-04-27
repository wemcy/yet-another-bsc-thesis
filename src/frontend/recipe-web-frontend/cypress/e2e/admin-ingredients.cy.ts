describe('Admin Ingredients Page [U12A, U12B, U12C]', () => {
    const ingredientName = `Cypress Ingredient ${Date.now()}`
    const updatedIngredientName = `${ingredientName} Updated`

    context('Support - access control', () => {
        it('redirects an unauthenticated user to /login', () => {
            cy.visit('/admin/ingredients')
            cy.url().should('include', '/login')
        })

        it('does not allow a regular user to access the admin ingredient page', () => {
            cy.loginAsTestUser()
            cy.visit('/admin/ingredients')
            cy.url().should('not.include', '/admin/ingredients')
            cy.contains('Hozzávalók kezelése').should('not.exist')
        })
    })

    context('U12A - Ingredient search and load', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/admin/ingredients')
            cy.contains('h1', 'Hozzávalók kezelése').should('be.visible')
        })

        it('searches for an ingredient and loads it into the editor', () => {
            cy.get('[data-cy="admin-ingredient-name"]').type(ingredientName)
            cy.contains('label', 'Glutén').find('input[type="checkbox"]').check()
            cy.contains('button', 'Hozzávaló létrehozása').click()

            cy.get('[data-cy="admin-ingredient-search"]').clear().type(ingredientName)
            cy.get('[data-cy="admin-search-button"]').click()
            cy.get('[data-cy="admin-ingredient-result"]')
                .contains(ingredientName)
                .should('be.visible')
                .click()

            cy.contains(' jelenleg szerkesztés alatt áll.').should('be.visible')
            cy.get('[data-cy="admin-ingredient-name"]').should('have.value', ingredientName)
            cy.contains('button', 'Módosítás mentése').should('be.visible')
            cy.contains('button', 'Hozzávaló törlése').should('be.visible')
            cy.contains('button', ingredientName).should('be.visible')
        })
    })

    context('U12B - Empty ingredient search handling', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/admin/ingredients')
            cy.contains('h1', 'Hozzávalók kezelése').should('be.visible')
        })

        it('does not run a meaningful search for an empty query and shows feedback', () => {
            cy.get('[data-cy="admin-ingredient-search"]').clear()
            cy.get('[data-cy="admin-search-button"]').click()

            cy.contains('Adj meg legalább egy karaktert a kereséshez.').should('be.visible')
        })
    })

    context('U12C - Modify ingredient allergen information', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/admin/ingredients')
            cy.contains('h1', 'Hozzávalók kezelése').should('be.visible')

            cy.get('[data-cy="admin-ingredient-name"]').type(ingredientName)
            cy.contains('label', 'Glutén').find('input[type="checkbox"]').check()
            cy.contains('button', 'Hozzávaló létrehozása').click()
            cy.get('[data-cy="admin-ingredient-search"]').clear().type(ingredientName)
            cy.get('[data-cy="admin-search-button"]').click()
            cy.get('[data-cy="admin-ingredient-result"]')
                .contains(ingredientName)
                .should('be.visible')
                .click()
        })

        it('updates an existing ingredient and refreshes the displayed data', () => {
            cy.get('[data-cy="admin-ingredient-name"]').clear().type(updatedIngredientName)
            cy.contains('label', 'Tej').find('input[type="checkbox"]').check()
            cy.contains('button', 'Módosítás mentése').click()

            cy.get('[data-cy="admin-ingredient-name"]').should('have.value', updatedIngredientName)
            cy.contains('button', 'Módosítás mentése').should('be.visible')
            cy.contains('button', 'Hozzávaló törlése').should('be.visible')

            cy.get('[data-cy="admin-search-button"]').click()
            cy.get('[data-cy="admin-ingredient-result"]')
                .contains(updatedIngredientName)
                .should('be.visible')
                .click()
            cy.get('[data-cy="admin-ingredient-name"]').should('have.value', updatedIngredientName)
            cy.contains('label', 'Glutén').find('input[type="checkbox"]').should('be.checked')
            cy.contains('label', 'Tej').find('input[type="checkbox"]').should('be.checked')
        })
    })

    context('Support - cleanup', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/admin/ingredients')
            cy.contains('h1', 'Hozzávalók kezelése').should('be.visible')

            cy.get('[data-cy="admin-ingredient-search"]').clear().type(updatedIngredientName)
            cy.get('[data-cy="admin-search-button"]').click()
        })

        it('deletes the temporary ingredient and verifies it no longer appears in search', () => {
            cy.contains('button', updatedIngredientName).then(($button) => {
                if ($button.length > 0) {
                    cy.wrap($button).click()
                    cy.contains('button', 'Hozzávaló törlése').click()
                    cy.get('[data-cy="admin-ingredient-name"]').should('have.value', '')
                    cy.contains('button', 'Hozzávaló létrehozása').should('be.visible')
                    cy.contains('button', 'Hozzávaló törlése').should('not.exist')
                }
            })

            cy.get('[data-cy="admin-ingredient-search"]').clear().type(updatedIngredientName)
            cy.get('[data-cy="admin-search-button"]').click()
            cy.contains('Nincs találat erre a keresésre.').should('be.visible')
        })
    })
})
