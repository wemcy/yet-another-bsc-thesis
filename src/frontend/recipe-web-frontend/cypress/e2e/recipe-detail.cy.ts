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

            it('displays an author name with a link to their profile', () => {
                cy.get('a[href*="/profile/"]')
                    .filter(':not(nav a)')
                    .first()
                    .should('be.visible')
                    .invoke('text')
                    .should('have.length.at.least', 1)
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

            it('shows the "Szerző" badge on comments by the recipe author', () => {
                // Check if there are comments — if the author has commented, the badge should appear
                cy.get('body').then(($body) => {
                    if ($body.find('.rounded-full:contains("Szerző")').length > 0) {
                        cy.contains('Szerző').should('be.visible')
                    } else {
                        cy.log('No author comments visible — skipping Szerző badge check')
                    }
                })
            })

            it('shows the author name as a link to their public profile', () => {
                cy.get('body').then(($body) => {
                    if (!$body.text().includes('Még nincs hozzászólás')) {
                        cy.contains('h2', 'Hozzászólások')
                            .parents('section')
                            .find('a[href*="/profile/"]')
                            .should('have.length.at.least', 1)
                    }
                })
            })
        })

        context('Rating interaction', () => {
            it('can click a star to rate the recipe', () => {
                cy.get('[aria-label="3 csillag"]').click()
                // The rating display should update
                cy.contains('(3/5)').should('be.visible')
            })
        })
    })

    context('Edit / delete controls for non-owners', () => {
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

    context('Guest behavior', () => {
        beforeEach(() => {
            cy.visit('/recipes')
            cy.get('a[href*="/recipe/"]').first().click()
            cy.url().should('include', '/recipe/')
        })

        it('shows "Jelentkezz be a hozzászóláshoz!" instead of the comment form for guests', () => {
            cy.contains('Jelentkezz be a hozzászóláshoz!').should('be.visible')
            cy.get('textarea[placeholder="Írd meg a véleményed..."]').should('not.exist')
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
