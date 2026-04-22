describe('Public Profile Page [U08B]', () => {
    // Helper: get the test user's ID by registering/fetching via API
    function getTestUserId(): Cypress.Chainable<string> {
        const email = Cypress.env('testUserEmail')
        const password = Cypress.env('testUserPassword')
        const displayName = Cypress.env('testUserDisplayName')

        // Ensure test user exists
        cy.request({
            method: 'POST',
            url: '/api/auth/register',
            body: { email, password, displayName },
            failOnStatusCode: false,
        })

        // Log in as test user to get their ID
        return cy
            .request({
                method: 'POST',
                url: '/api/auth/login',
                body: { email, password },
            })
            .then((resp) => resp.body.id as string)
    }

    // Navigate to a public profile via Vue Router (no full page reload)
    // so the Pinia store retains the admin roles from the login response.
    function navigateToPublicProfile(userId: string) {
        cy.get('[data-cy="app-root"]').then(($el) => {
            const app = ($el[0] as any).__vue_app__
            const router = app.config.globalProperties.$router
            router.push(`/profile/${userId}`)
        })
        cy.url().should('include', `/profile/${userId}`)
    }

    context('U08B - Másik felhasználó profiljának megtekintése', () => {
        let testUserId: string

        before(() => {
            getTestUserId().then((id) => {
                testUserId = id
            })
        })

        beforeEach(() => {
            cy.login()
            cy.visit(`/profile/${testUserId}`)
        })

        it('displays the user display name', () => {
            cy.contains(Cypress.env('testUserDisplayName')).should('be.visible')
        })

        it('displays the "Receptjei" heading', () => {
            cy.contains('h3', 'Receptjei').should('be.visible')
        })

        it('shows recipe cards or the empty state message', () => {
            // Wait for the recipes section to finish loading
            cy.contains('h3', 'Receptjei').should('be.visible')
            // Assert that either recipes or the empty state message appears
            cy.get('main', { timeout: 10000 }).should(($main) => {
                const hasRecipes = $main.find('a[href*="/recipe/"]').length > 0
                const hasEmpty = $main
                    .text()
                    .includes('Ennek a felhasználónak még nincsenek receptjei.')
                expect(hasRecipes || hasEmpty).to.be.true
            })
        })
    })

    context('Support - redirect when visiting own public profile', () => {
        it('redirects to /profile when the logged-in user visits their own public profile', () => {
            // Login as admin, get admin ID, then visit /profile/<adminId>
            cy.request({
                method: 'POST',
                url: '/api/auth/login',
                body: {
                    email: Cypress.env('userEmail'),
                    password: Cypress.env('userPassword'),
                },
            }).then((resp) => {
                const adminId = resp.body.id
                cy.login()
                cy.visit(`/profile/${adminId}`)
                cy.url().should('include', '/profile')
                cy.url().should('not.include', `/profile/${adminId}`)
            })
        })
    })

    context('Support - error states', () => {
        it('shows an error message for a non-existent user', () => {
            cy.login()
            cy.visit('/profile/00000000-0000-0000-0000-000000000000')
            cy.contains('A profil nem található.').should('be.visible')
        })
    })

    context('Support - admin controls on public profile', () => {
        let testUserId: string

        before(() => {
            getTestUserId().then((id) => {
                testUserId = id
            })
        })

        it('shows "Profil törlése" and "Adminná tétel" buttons for admin', () => {
            cy.login()
            // Use in-app navigation to preserve Pinia admin roles
            navigateToPublicProfile(testUserId)
            cy.contains('button', 'Profil törlése').should('be.visible')
            cy.contains('button', 'Adminná tétel').should('be.visible')
        })

        it('opens the delete profile dialog when clicking "Profil törlése"', () => {
            cy.login()
            navigateToPublicProfile(testUserId)
            cy.contains('button', 'Profil törlése').click()
            cy.contains('h2', 'Profil törlése').should('be.visible')
            cy.contains('Biztosan törölni szeretnéd ezt a profilt?').should('be.visible')
        })

        it('closes the delete dialog when Mégse is clicked', () => {
            cy.login()
            navigateToPublicProfile(testUserId)
            cy.contains('button', 'Profil törlése').click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Biztosan törölni szeretnéd ezt a profilt?').should('not.exist')
        })

        it('opens the make-admin dialog when clicking "Adminná tétel"', () => {
            cy.login()
            navigateToPublicProfile(testUserId)
            cy.contains('button', 'Adminná tétel').click()
            cy.contains('h2', 'Adminná tétel').should('be.visible')
            cy.contains('Biztosan admin jogosultságot szeretnél adni').should('be.visible')
        })

        it('closes the make-admin dialog when Mégse is clicked', () => {
            cy.login()
            navigateToPublicProfile(testUserId)
            cy.contains('button', 'Adminná tétel').click()
            cy.contains('button', 'Mégse').click()
            cy.contains('Biztosan admin jogosultságot szeretnél adni').should('not.exist')
        })
    })

    context('Support - non-admin cannot see admin controls', () => {
        it('does not show admin buttons for a regular user', () => {
            // Get admin ID to visit their profile as a regular user
            cy.request({
                method: 'POST',
                url: '/api/auth/login',
                body: {
                    email: Cypress.env('userEmail'),
                    password: Cypress.env('userPassword'),
                },
            }).then((resp) => {
                const adminId = resp.body.id
                cy.loginAsTestUser()
                cy.visit(`/profile/${adminId}`)
                cy.contains(Cypress.env('userDisplayName')).should('be.visible')
                cy.contains('button', 'Profil törlése').should('not.exist')
                cy.contains('button', 'Adminná tétel').should('not.exist')
            })
        })
    })
})
