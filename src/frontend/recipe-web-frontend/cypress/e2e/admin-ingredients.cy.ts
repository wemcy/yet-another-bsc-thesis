describe('Admin Ingredients Page', () => {
    const ingredientName = `Cypress Ingredient ${Date.now()}`
    const updatedIngredientName = `${ingredientName} Updated`

    context('Access control', () => {
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

    context('Ingredient management', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/admin/ingredients')
            cy.contains('h1', 'Hozzávalók kezelése').should('be.visible')
        })

        it('creates, updates, searches, and deletes an ingredient', () => {
            cy.get('#ingredient-name').type(ingredientName)
            cy.contains('label', 'Glutén').find('input[type="checkbox"]').check()
            cy.contains('button', 'Hozzávaló létrehozása').click()

            cy.get('#ingredient-name').should('have.value', ingredientName)
            cy.contains('button', 'Módosítás mentése').should('be.visible')
            cy.contains('button', 'Hozzávaló törlése').should('be.visible')
            cy.contains('button', ingredientName).should('be.visible')

            cy.contains('button', 'Keresés').click()
            cy.contains('button', ingredientName).should('be.visible').click()

            cy.contains('betöltve szerkesztésre.').should('be.visible')
            cy.get('#ingredient-name').clear().type(updatedIngredientName)
            cy.contains('label', 'Tej').find('input[type="checkbox"]').check()
            cy.contains('button', 'Módosítás mentése').click()

            cy.contains('A hozzávaló módosítása sikeres volt.').should('be.visible')
            cy.get('#ingredient-name').should('have.value', updatedIngredientName)

            cy.contains('button', 'Keresés').click()
            cy.contains('button', updatedIngredientName).should('be.visible').click()
            cy.contains('Glutén').should('be.visible')
            cy.contains('Tej').should('be.visible')

            cy.contains('button', 'Hozzávaló törlése').click()
            cy.contains(`"${updatedIngredientName}" hozzávaló törölve lett.`).should('be.visible')

            cy.get('input[placeholder="pl. zabpehely"]').clear().type(updatedIngredientName)
            cy.contains('button', 'Keresés').click()
            cy.contains('Nincs találat erre a keresésre.').should('be.visible')
        })
    })
})
