describe('Profile Page', () => {
    context('Auth guard', () => {
        it('redirects an unauthenticated user to /login', () => {
            cy.visit('/profile')
            cy.url().should('include', '/login')
        })

        it('includes the redirect query param so the user returns after login', () => {
            cy.visit('/profile')
            cy.url().should('include', 'redirect=')
            cy.url().should('include', 'profile')
        })
    })

    context('Profile information display', () => {
        beforeEach(() => {
            cy.login()
            cy.contains('nav a', 'ReceptApp').click() // ensure state
            cy.visit('/profile')
        })

        it('displays the Név (name) label and value', () => {
            cy.contains('label', 'Név').should('be.visible')
            cy.contains('p', Cypress.env('userDisplayName')).should('be.visible')
        })

        it('displays the Email label and value', () => {
            cy.contains('label', 'Email').should('be.visible')
            cy.contains('p', Cypress.env('userEmail')).should('be.visible')
        })

        it('displays the masked password (********)', () => {
            cy.contains('label', 'Jelszó').should('be.visible')
            cy.contains('p', '********').should('be.visible')
        })

        it('displays the Regisztráció dátuma label', () => {
            cy.contains('label', 'Regisztráció dátuma').should('be.visible')
        })

        it('shows the Szerkesztés (edit) button', () => {
            cy.contains('button', 'Szerkesztés').should('be.visible')
        })

        it('shows the Profil törlése (delete profile) button', () => {
            cy.contains('button', 'Profil törlése').should('be.visible')
        })
    })

    context('Edit mode', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
            cy.contains('button', 'Szerkesztés').click()
        })

        it('shows editable name input field', () => {
            cy.get('input')
                .filter((_, el) => {
                    return (el as HTMLInputElement).value === Cypress.env('userDisplayName')
                })
                .should('exist')
        })

        it('shows editable email input field', () => {
            cy.get('input')
                .filter((_, el) => {
                    return (el as HTMLInputElement).value === Cypress.env('userEmail')
                })
                .should('exist')
        })

        it('shows password input with placeholder', () => {
            cy.get('input[type="password"][placeholder*="Új jelszó"]').should(
                'have.length.at.least',
                1,
            )
        })

        it('shows password confirm input', () => {
            cy.contains('label', 'Jelszó megerősítése').should('be.visible')
            cy.get('input[type="password"][placeholder*="ismét"]').should('exist')
        })

        it('shows the file upload input for avatar', () => {
            cy.get('input[type="file"][accept*="image"]').should('exist')
        })

        it('shows Mentés (save) and Mégse (cancel) buttons', () => {
            cy.contains('button', 'Mentés').should('be.visible')
            cy.contains('button', 'Mégse').should('be.visible')
        })

        it('hides the Szerkesztés button in edit mode', () => {
            cy.contains('button', 'Szerkesztés').should('not.exist')
        })

        it('cancels editing and restores view mode when Mégse is clicked', () => {
            cy.contains('button', 'Mégse').click()
            cy.contains('button', 'Szerkesztés').should('be.visible')
            cy.contains('p', Cypress.env('userDisplayName')).should('be.visible')
        })
    })

    context('Profile validation in edit mode', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
            cy.contains('button', 'Szerkesztés').click()
        })

        it('shows error when name is cleared', () => {
            // Find the name input (first text input with the display name) and clear it
            cy.get('input')
                .filter((_, el) => {
                    return (el as HTMLInputElement).value === Cypress.env('userDisplayName')
                })
                .clear()
            cy.contains('button', 'Mentés').click()
            cy.contains('A név kötelező.').should('be.visible')
        })

        it('shows error when passwords do not match', () => {
            cy.get('input[type="password"][placeholder*="Új jelszó"]').first().type('NewPass123!')
            cy.get('input[type="password"][placeholder*="ismét"]').type('DifferentPass!')
            cy.contains('button', 'Mentés').click()
            cy.contains('A jelszavak nem egyeznek.').should('be.visible')
        })

        it('shows error when password is too short', () => {
            cy.get('input[type="password"][placeholder*="Új jelszó"]').first().type('Ab1')
            cy.get('input[type="password"][placeholder*="ismét"]').type('Ab1')
            cy.contains('button', 'Mentés').click()
            cy.contains('A jelszónak legalább 6 karakteresnek kell lennie.').should('be.visible')
        })
    })

    context('Delete profile dialog', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
        })

        it('opens the delete confirmation dialog', () => {
            cy.contains('button', 'Profil törlése').click()
            cy.contains('h2', 'Profil törlése').should('be.visible')
            cy.contains('Biztosan törölni szeretnéd a profilodat?').should('be.visible')
        })

        it('closes the dialog when Mégse is clicked', () => {
            cy.contains('button', 'Profil törlése').click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Biztosan törölni szeretnéd a profilodat?').should('not.exist')
        })

        it('shows the Igen, törlöm confirmation button', () => {
            cy.contains('button', 'Profil törlése').click()
            cy.contains('button', 'Igen, törlöm').should('be.visible')
        })
    })

    context('Own recipes section', () => {
        beforeEach(() => {
            cy.login()
            cy.visit('/profile')
        })

        it('displays the "Saját receptjeim" heading', () => {
            cy.contains('h3', 'Saját receptjeim').should('be.visible')
        })

        it('shows recipe cards or the empty state message', () => {
            cy.get('body').then(($body) => {
                if ($body.text().includes('Még nincs saját recepted.')) {
                    cy.contains('Még nincs saját recepted.').should('be.visible')
                } else {
                    cy.get('a[href*="/recipe/"]').should('have.length.at.least', 1)
                }
            })
        })
    })
})
