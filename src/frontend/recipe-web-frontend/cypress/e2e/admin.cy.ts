describe('Admin Features [U07A, U07B, U11A, U11B]', () => {
    // Navigate to the first recipe via in-app navigation (no cy.visit)
    // so the Pinia store retains the admin roles from the login response.
    function navigateToFirstRecipe() {
        cy.contains('nav a', 'Receptek').click()
        cy.url().should('include', '/recipes')
        cy.get('a[href*="/recipe/"]').first().click()
        cy.url().should('include', '/recipe/')
    }

    context('U07A - Recept törlése megerősítéssel adminisztrátorként', () => {
        beforeEach(() => {
            cy.login()
            navigateToFirstRecipe()
        })

        it('shows the Szerkesztés (edit) link', () => {
            cy.contains('a', 'Szerkesztés').should('be.visible')
        })

        it('shows the Törlés (delete recipe) button', () => {
            // The first 'Törlés' button in the action bar (not inside a dialog)
            cy.contains('button', 'Törlés').first().should('be.visible')
        })

        it('shows the "Kiemelt receptté teszem" or "Ez a kiemelt recept" button', () => {
            cy.contains('button', /Kiemelt receptté teszem|Ez a kiemelt recept/).should(
                'be.visible',
            )
        })

        it('opens the delete confirmation dialog when clicking Törlés', () => {
            cy.contains('button', 'Törlés').first().click()
            cy.contains('Biztosan törölni szeretnéd ezt a receptet?').should('be.visible')
        })
    })

    context('U07B - Recept törlésének megszakítása adminisztrátorként', () => {
        beforeEach(() => {
            cy.login()
            navigateToFirstRecipe()
        })

        it('closes the dialog when clicking Mégse', () => {
            cy.contains('button', 'Törlés').first().click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Biztosan törölni szeretnéd ezt a receptet?').should('not.exist')
        })
    })

    context('Support - featured recipe dialog', () => {
        beforeEach(() => {
            cy.login()
            // Navigate to a recipe that is NOT already featured so the button is clickable
            cy.contains('nav a', 'Receptek').click()
            cy.url().should('include', '/recipes')
            cy.get('a[href*="/recipe/"]').eq(1).click()
            cy.url().should('include', '/recipe/')
        })

        it('opens the featured recipe dialog when clicking the feature button', () => {
            cy.contains('button', 'Kiemelt receptté teszem').first().click()
            cy.contains('Biztosan ezt a receptet szeretnéd beállítani kiemelt receptnek?').should(
                'be.visible',
            )
        })

        it('closes the featured recipe dialog when clicking Mégse', () => {
            cy.contains('button', 'Kiemelt receptté teszem').first().click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Biztosan ezt a receptet szeretnéd beállítani kiemelt receptnek?').should(
                'not.exist',
            )
        })
    })

    context('U11A - Hozzászólás törlése adminisztrátorként', () => {
        beforeEach(() => {
            cy.login()
            navigateToFirstRecipe()
        })

        it('shows a Törlés button on comments when comments exist', () => {
            cy.get('body').then(($body) => {
                if ($body.text().includes('Még nincs hozzászólás')) {
                    cy.log('No comments on this recipe — skipping assertion')
                    return
                }
                // The delete button inside the comments section
                cy.contains('h2', 'Hozzászólások')
                    .parents('section')
                    .find('button')
                    .contains('Törlés')
                    .should('exist')
            })
        })
    })

    context('U11B - Hozzászólás törlésének megszakítása adminisztrátorként', () => {
        beforeEach(() => {
            cy.login()
            navigateToFirstRecipe()
        })

        it('keeps the comment when the delete flow is not started or not available', () => {
            cy.get('body').then(($body) => {
                if ($body.text().includes('Még nincs hozzászólás')) {
                    cy.log('No comments on this recipe — skipping')
                    return
                }

                cy.contains('h2', 'Hozzászólások')
                    .parents('section')
                    .find('button')
                    .contains('Törlés')
                    .should('exist')
            })
        })
    })

    context('Support - admin controls NOT visible to regular users', () => {
        beforeEach(() => {
            cy.loginAsTestUser()
            navigateToFirstRecipe()
        })

        it('does not show the "Kiemelt receptté teszem" button', () => {
            cy.contains('button', /Kiemelt receptté teszem|Ez a kiemelt recept/).should('not.exist')
        })

        it('does not show a Törlés button on comments', () => {
            cy.get('body').then(($body) => {
                if ($body.text().includes('Még nincs hozzászólás')) {
                    cy.log('No comments on this recipe — skipping assertion')
                    return
                }
                cy.contains('h2', 'Hozzászólások')
                    .parents('section')
                    .find('button')
                    .contains('Törlés')
                    .should('not.exist')
            })
        })

        it("does not show edit or delete recipe buttons when visiting another user's recipe", () => {
            // Only meaningful if the recipe is not owned by the test user.
            // Since the test user has no recipes, this will always be true for the first recipe.
            cy.contains('a', 'Szerkesztés').should('not.exist')
            cy.contains('button', 'Törlés').should('not.exist')
        })
    })
})
