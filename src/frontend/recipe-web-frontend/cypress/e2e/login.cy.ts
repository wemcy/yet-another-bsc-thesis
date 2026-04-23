const VALID_USER_NAME = 'Teszt'

describe('Auth - Login [support]', () => {
    beforeEach(() => {
        cy.visit('/login')
    })

    context('Support - login form structure', () => {
        it('shows the login tab as active by default', () => {
            cy.get('[data-cy="login-email"]').should('be.visible')
            cy.get('[data-cy="login-password"]').should('be.visible')
            cy.get('[data-cy="login-submit"]').should('be.visible')
        })

        it('has a "Remember me" checkbox', () => {
            cy.contains('label', 'Emlékezz rám').find('input[type="checkbox"]').should('exist')
        })
    })

    context('Support - login validation', () => {
        it('shows an error when email is empty on submit', () => {
            cy.get('[data-cy="login-submit"]').click()
            cy.contains('p', 'Az e-mail cím kötelező.').should('be.visible')
        })

        it('shows an error when password is empty on submit', () => {
            cy.get('[data-cy="login-email"]').type('test@example.com')
            cy.get('[data-cy="login-submit"]').click()
            cy.contains('p', 'A jelszó kötelező.').should('be.visible')
        })
    })

    context('Support - login API interactions', () => {
        it('redirects away from /login after a successful login', () => {
            cy.get('[data-cy="login-email"]').type(Cypress.env('userEmail'))
            cy.get('[data-cy="login-password"]').type(Cypress.env('userPassword'))
            cy.get('[data-cy="login-submit"]').click()
            cy.url().should('not.include', '/login')
        })

        it('displays a server error message on failed login', () => {
            cy.get('[data-cy="login-email"]').type('nonexistent@invalid.example.com')
            cy.get('[data-cy="login-password"]').type('WrongPassword123')
            cy.get('[data-cy="login-submit"]').click()
            cy.get('.bg-red-50').should('be.visible')
        })
    })
})

describe('Auth - Register [U01A, U01B, U01C]', () => {
    beforeEach(() => {
        cy.visit('/login')
        cy.contains('button', 'Regisztráció').first().click()
    })

    context('Support - register form structure', () => {
        it('shows all register form fields', () => {
            cy.get('[data-cy="register-name"]').should('be.visible')
            cy.get('[data-cy="register-email"]').should('be.visible')
            cy.get('[data-cy="register-password"]').should('be.visible')
            cy.get('[data-cy="register-password-confirm"]').should('be.visible')
            cy.get('[data-cy="register-accept-terms"]').should('be.visible')
            cy.get('[data-cy="register-submit"]').should('be.visible')
        })
    })

    context('U01A - Successful registration', () => {
        it('redirects away from /login after a successful registration', () => {
            // Use a unique email so repeated test runs do not collide
            const uniqueEmail = `cypress+${Date.now()}@example.com`

            cy.get('[data-cy="register-name"]').type('Cypress Teszt')
            cy.get('[data-cy="register-email"]').type(uniqueEmail)
            cy.get('[data-cy="register-password"]').type('Password123!')
            cy.get('[data-cy="register-password-confirm"]').type('Password123!')
            cy.get('[data-cy="register-accept-terms"]').check()
            cy.get('[data-cy="register-submit"]').click()
            cy.url().should('not.include', '/login')
        })
    })

    context('U01B - Handling already used email', () => {
        it('shows a server-side error when the email is already in use', () => {
            cy.get('[data-cy="register-name"]').type('Cypress Foglalt')
            cy.get('[data-cy="register-email"]').type(Cypress.env('userEmail'))
            cy.get('[data-cy="register-password"]').type('Password123!')
            cy.get('[data-cy="register-password-confirm"]').type('Password123!')
            cy.get('[data-cy="register-accept-terms"]').check()
            cy.get('[data-cy="register-submit"]').click()
            cy.get('.bg-red-50').should('be.visible')
        })
    })

    context('U01C - Register with invalid data', () => {
        it('shows an error when name is empty', () => {
            cy.get('[data-cy="register-submit"]').click()
            cy.contains('p', 'A név kötelező.').should('be.visible')
        })

        it('shows an error when email is empty', () => {
            cy.get('[data-cy="register-name"]').type('Teszt Felhasználó')
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'Az e-mail cím kötelező.').should('be.visible')
        })

        it('shows an error when the password is shorter than 8 characters', () => {
            cy.get('[data-cy="register-name"]').type(VALID_USER_NAME)
            cy.get('[data-cy="register-email"]').type('test@example.com')
            cy.get('[data-cy="register-password"]').type('rövid')
            cy.get('[data-cy="register-password-confirm"]').type('rövid')
            cy.get('[data-cy="register-submit"]').click()
            cy.contains('p', 'A jelszó legalább 8 karakter legyen.').should('be.visible')
        })

        it('shows an error when the passwords do not match', () => {
            cy.get('[data-cy="register-name"]').type(VALID_USER_NAME)
            cy.get('[data-cy="register-email"]').type('test@example.com')
            cy.get('[data-cy="register-password"]').type('Password123')
            cy.get('[data-cy="register-password-confirm"]').type('DifferentPass')
            cy.get('[data-cy="register-submit"]').click()
            cy.contains('p', 'A két jelszó nem egyezik.').should('be.visible')
        })

        it('shows an error when terms are not accepted', () => {
            cy.get('[data-cy="register-name"]').type(VALID_USER_NAME)
            cy.get('[data-cy="register-email"]').type('test@example.com')
            cy.get('[data-cy="register-password"]').type('Password123')
            cy.get('[data-cy="register-password-confirm"]').type('Password123')
            cy.get('[data-cy="register-submit"]').click()
            cy.contains('p', 'A regisztrációhoz fogadd el a feltételeket.').should('be.visible')
        })
    })
})
