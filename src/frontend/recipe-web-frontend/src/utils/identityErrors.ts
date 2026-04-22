import { ResponseError } from 'recipe-api-client'
import { isObject, isString } from '@/utils/typeGuards'

interface IdentityError {
    code: string
    description: string
}

const IDENTITY_ERROR_MESSAGES: Record<string, string> = {
    DefaultError: 'Ismeretlen hiba történt.',
    ConcurrencyFailure: 'Párhuzamos módosítás történt, próbáld újra.',
    PasswordMismatch: 'Hibás jelszó.',
    InvalidToken: 'Érvénytelen token.',
    LoginAlreadyAssociated: 'Ez a bejelentkezési mód már hozzá van rendelve egy fiókhoz.',
    InvalidUserName: 'Érvénytelen felhasználónév.',
    InvalidEmail: 'Érvénytelen e-mail cím.',
    DuplicateUserName: 'Ez a felhasználónév már foglalt.',
    DuplicateEmail: 'Ez az e-mail cím már regisztrálva van.',
    PasswordTooShort: 'A jelszó túl rövid.',
    PasswordRequiresUniqueChars: 'A jelszónak több egyedi karaktert kell tartalmaznia.',
    PasswordRequiresNonAlphanumeric:
        'A jelszónak tartalmaznia kell legalább egy speciális karaktert (pl. !@#$).',
    PasswordRequiresDigit: 'A jelszónak tartalmaznia kell legalább egy számjegyet (0-9).',
    PasswordRequiresLower: 'A jelszónak tartalmaznia kell legalább egy kisbetűt (a-z).',
    PasswordRequiresUpper: 'A jelszónak tartalmaznia kell legalább egy nagybetűt (A-Z).',
    UserAlreadyHasPassword: 'A felhasználónak már van jelszava.',
    UserLockoutNotEnabled: 'A fiókzárolás nem engedélyezett.',
    InvalidCredentials: 'Érvénytelen e-mail cím vagy jelszó.',
    InvalidEmailOrPassword: 'Érvénytelen e-mail cím vagy jelszó.',
}

function isIdentityErrorArray(body: unknown): body is IdentityError[] {
    return (
        Array.isArray(body) &&
        body.length > 0 &&
        body.every((item) => isObject(item) && isString(item.code) && isString(item.description))
    )
}

function isErrorObject(body: unknown): body is { error: string } {
    return isObject(body) && isString(body.error)
}

function translateIdentityErrors(errors: IdentityError[]): string {
    return errors.map((e) => IDENTITY_ERROR_MESSAGES[e.code] ?? e.description).join(' ')
}

export async function toErrorMessage(error: unknown): Promise<string> {
    if (error instanceof ResponseError) {
        if (error.response.status === 401) {
            return 'Érvénytelen e-mail cím vagy jelszó.'
        }
        try {
            const body = await error.response.clone().json()
            if (isIdentityErrorArray(body)) {
                return translateIdentityErrors(body)
            }
            if (isErrorObject(body)) {
                return body.error
            }
        } catch {
            // body was not JSON — fall through
        }
    }
    if (error instanceof Error) return error.message
    return 'Váratlan hiba történt.'
}
