describe('Auth – Login', () => {
    beforeEach(() => {
        cy.visit('/login')
    })

    context('Form structure', () => {
        it('shows the login tab as active by default', () => {
            cy.get('#login-email').should('be.visible')
            cy.get('#login-password').should('be.visible')
            cy.contains('button[type="submit"]', 'Bejelentkezés').should('be.visible')
        })

        it('has a "Remember me" checkbox', () => {
            cy.contains('label', 'Emlékezz rám').find('input[type="checkbox"]').should('exist')
        })
    })

    context('Client-side validation', () => {
        it('shows an error when email is empty on submit', () => {
            cy.contains('button[type="submit"]', 'Bejelentkezés').click()
            cy.contains('p', 'Az e-mail cím kötelező.').should('be.visible')
        })

        it('shows an error when password is empty on submit', () => {
            cy.get('#login-email').type('test@example.com')
            cy.contains('button[type="submit"]', 'Bejelentkezés').click()
            cy.contains('p', 'A jelszó kötelező.').should('be.visible')
        })
    })

    context('API interactions', () => {
        it('redirects away from /login after a successful login', () => {
            cy.get('#login-email').type(Cypress.env('userEmail'))
            cy.get('#login-password').type(Cypress.env('userPassword'))
            cy.contains('button[type="submit"]', 'Bejelentkezés').click()
            cy.url().should('not.include', '/login')
        })

        it('displays a server error message on failed login', () => {
            cy.get('#login-email').type('nonexistent@invalid.example.com')
            cy.get('#login-password').type('WrongPassword123')
            cy.contains('button[type="submit"]', 'Bejelentkezés').click()
            cy.get('.bg-red-50').should('be.visible')
        })
    })
})

describe('Auth – Register', () => {
    beforeEach(() => {
        cy.visit('/login')
        cy.contains('button', 'Regisztráció').first().click()
    })

    context('Form structure', () => {
        it('shows all register form fields', () => {
            cy.get('#register-name').should('be.visible')
            cy.get('#register-email').should('be.visible')
            cy.get('#register-password').should('be.visible')
            cy.get('#register-password-confirm').should('be.visible')
            cy.contains('label', 'Elfogadom a feltételeket').should('be.visible')
            cy.contains('button[type="submit"]', 'Regisztráció').should('be.visible')
        })
    })

    context('Client-side validation', () => {
        it('shows an error when name is empty', () => {
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'A név kötelező.').should('be.visible')
        })

        it('shows an error when email is empty', () => {
            cy.get('#register-name').type('Teszt Felhasználó')
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'Az e-mail cím kötelező.').should('be.visible')
        })

        it('shows an error when the password is shorter than 8 characters', () => {
            cy.get('#register-name').type('Teszt')
            cy.get('#register-email').type('test@example.com')
            cy.get('#register-password').type('rövid')
            cy.get('#register-password-confirm').type('rövid')
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'A jelszó legalább 8 karakter legyen.').should('be.visible')
        })

        it('shows an error when the passwords do not match', () => {
            cy.get('#register-name').type('Teszt')
            cy.get('#register-email').type('test@example.com')
            cy.get('#register-password').type('Password123')
            cy.get('#register-password-confirm').type('DifferentPass')
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'A két jelszó nem egyezik.').should('be.visible')
        })

        it('shows an error when terms are not accepted', () => {
            cy.get('#register-name').type('Teszt')
            cy.get('#register-email').type('test@example.com')
            cy.get('#register-password').type('Password123')
            cy.get('#register-password-confirm').type('Password123')
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.contains('p', 'A regisztrációhoz fogadd el a feltételeket.').should('be.visible')
        })
    })

    context('API interactions', () => {
        it('redirects away from /login after a successful registration', () => {
            // Use a unique email so repeated test runs do not collide
            const uniqueEmail = `cypress+${Date.now()}@example.com`

            cy.get('#register-name').type('Cypress Teszt')
            cy.get('#register-email').type(uniqueEmail)
            cy.get('#register-password').type('Password123!')
            cy.get('#register-password-confirm').type('Password123!')
            cy.contains('label', 'Elfogadom a feltételeket').find('input[type="checkbox"]').check()
            cy.contains('button[type="submit"]', 'Regisztráció').click()
            cy.url().should('not.include', '/login')
        })
    })
})
