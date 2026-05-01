describe('Recipe Detail Page [U04A, U04B, U05A, U05B, U07A, U07B, U10A, U10B]', () => {
    context('U05B - Non-existent recipe details', () => {
        it('shows the "not found" message when the recipe does not exist', () => {
            // Use a well-formed but non-existent ID
            cy.visit('/recipe/00000000-0000-0000-0000-000000000000')
            cy.contains('A recept nem található.').should('be.visible')
        })
    })

    context('U05A - View recipe details', () => {
        beforeEach(() => {
            cy.login()
            cy.seedRecipes()

            // Navigate to the first available recipe from the listing page
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })
        afterEach(() => {
            cy.cleanupSeededRecipes()
         })
        it('displays a recipe title', () => {
            cy.get('h1, h2').first().invoke('text').should('have.length.at.least', 1)
        })

        it('displays an author name with a link to their profile', () => {
            cy.get('a[href*="/profile/"]')
                .filter(':not(nav a)')
                .first()
                .should('be.visible')
                .invoke('text')
                .should('have.length.at.least', 1)
        })

        it('displays at least one ingredient', () => {
            cy.get('li, [class*="ingredient"]').should('have.length.at.least', 1)
        })

        it('displays at least one preparation step', () => {
            cy.get('ol li, [class*="step"]').should('have.length.at.least', 1)
        })

        it('shows the 5 star rating buttons', () => {
            cy.get('[aria-label*="csillag"]').should('have.length', 5)
        })

        it('shows the recipe image', () => {
            cy.get('img[alt="Image of the recipe"]').should('exist')
        })

        it('displays the comments heading', () => {
            cy.contains('h2', 'Hozzászólások').should('be.visible')
        })

        it('shows the comment form when logged in', () => {
            cy.get('form').should('exist')
        })
    })

    context('U10A - Add comment', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('shows the disabled submit button when the textarea is empty', () => {
            cy.contains('button', 'Hozzászólás küldése').should('be.disabled')
        })

        it('enables the submit button after typing a comment', () => {
            cy.get('textarea[placeholder="Írd meg a véleményed..."]').type('Teszt hozzászólás')
            cy.contains('button', 'Hozzászólás küldése').should('not.be.disabled')
        })

        it('posts a comment and displays it in the list', () => {
            const commentText = `Cypress teszt hozzászólás ${Date.now()}`
            cy.get('textarea[placeholder="Írd meg a véleményed..."]').type(commentText)
            cy.contains('button', 'Hozzászólás küldése').click()
            cy.contains(commentText).should('be.visible')
        })
    })

    context('U10B - Invalid comment handling', () => {
        beforeEach(() => {
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('shows the sign-in prompt instead of the comment form for guests', () => {
            cy.contains('Jelentkezz be a hozzászóláshoz!').should('be.visible')
            cy.get('textarea[placeholder="Írd meg a véleményed..."]').should('not.exist')
        })
    })

    context('U04A - Rate recipe', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('can click a star to rate the recipe', () => {
            cy.get('[aria-label="3 csillag"]').click()
            cy.contains('(3/5)').should('be.visible')
        })
    })

    context('U04B - Update previous rating', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('allows changing an existing rating by choosing another star', () => {
            cy.get('[aria-label="2 csillag"]').click()
            cy.contains('(2/5)').should('be.visible')
            cy.get('[aria-label="4 csillag"]').click()
            cy.contains('(4/5)').should('be.visible')
        })
    })

    context('U07A - Delete recipe with confirmation', () => {
        beforeEach(() => {
            cy.login()
            cy.seedRecipes()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })
        afterEach(() => {
            cy.cleanupSeededRecipes()
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
    })

    context('U07B - Abort recipe deletion', () => {
        beforeEach(() => {
            cy.login()
            cy.seedRecipes()
            cy.visit('/profile')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })
        afterEach(() => {
            cy.cleanupSeededRecipes()
    })

        it('closes the delete dialog when "Cancel" is clicked', () => {
            cy.contains('button', 'Törlés').click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Recept törlése').should('not.exist')
        })
    })

    context('Support - edit / delete controls for non-owners', () => {
        beforeEach(() => {
            cy.loginAsTestUser()
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('does not show an edit button for non-owners', () => {
            cy.contains('a', 'Szerkesztés').should('not.exist')
        })

        it('does not show a delete button for non-owners', () => {
            cy.contains('button', 'Törlés').should('not.exist')
        })
    })

    context('Support - guest behavior', () => {
        beforeEach(() => {
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('shows an alert when a guest tries to rate', () => {
            const stub = cy.stub()
            cy.on('window:alert', stub)
            cy.get('[aria-label="3 csillag"]').click()
            cy.then(() => {
                expect(stub).to.have.been.calledWith('Csak bejelentkezve értékelhetsz!')
            })
        })
    })
})
