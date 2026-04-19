describe('Recipe Detail Page', () => {
    context('Recipe not found', () => {
        it('shows the "not found" message when the recipe does not exist', () => {
            // Use a well-formed but non-existent ID
            cy.visit('/recipe/00000000-0000-0000-0000-000000000000')
            cy.contains('A recept nem található.').should('be.visible')
        })
    })

    context('Recipe loaded successfully', () => {
        beforeEach(() => {
            cy.login()
            // Navigate to the first available recipe from the listing page
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        context('Recipe header', () => {
            it('displays a recipe title', () => {
                cy.get('h1, h2').first().invoke('text').should('have.length.at.least', 1)
            })

            it('displays an author name', () => {
                cy.get('main').contains(/\S+/).should('exist')
            })
        })

        context('Ingredients and steps', () => {
            it('displays at least one ingredient', () => {
                cy.get('li, [class*="ingredient"]').should('have.length.at.least', 1)
            })

            it('displays at least one preparation step', () => {
                cy.get('ol li, [class*="step"]').should('have.length.at.least', 1)
            })
        })

        context('Sidebar', () => {
            it('shows the 5 star rating buttons', () => {
                cy.get('[aria-label*="csillag"]').should('have.length', 5)
            })

            it('shows the recipe image', () => {
                cy.get('img[alt="Image of the recipe"]').should('exist')
            })
        })

        context('Comments section', () => {
            it('displays the comments heading', () => {
                cy.contains('h2', 'Hozzászólások').should('be.visible')
            })

            it('shows the comment form when logged in', () => {
                cy.get('form').should('exist')
            })
        })

        context('Edit / delete controls for non-owners', () => {
            it('does not show an edit button for non-owners', () => {
                cy.contains('a', 'Szerkesztés').should('not.exist')
            })

            it('does not show a delete button for non-owners', () => {
                cy.contains('button', 'Törlés').should('not.exist')
            })
        })
    })

    context('Owner controls', () => {
        beforeEach(() => {
            cy.login()
            // Navigate to the first recipe the logged-in user owns via the profile page
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('shows the edit button for the recipe owner', () => {
            cy.contains('a', 'Szerkesztés').should('be.visible')
        })

        it('shows the delete button for the recipe owner', () => {
            cy.contains('button', 'Törlés').should('be.visible')
        })

        it('opens a confirmation dialog when the delete button is clicked', () => {
            cy.contains('button', 'Törlés').click()
            cy.contains('Recept törlése').should('be.visible')
            cy.contains('p', 'Ez a művelet nem visszavonható.').should('be.visible')
        })

        it('closes the delete dialog when "Mégse" is clicked', () => {
            cy.contains('button', 'Törlés').click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Recept törlése').should('not.exist')
        })
    })
})
