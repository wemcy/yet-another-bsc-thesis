export interface User {
    id: string // egyedi azonosító, pl. MongoDB ObjectId vagy UUID
    name: string
    email: string
    password?: string // opcionális, élesben általában nem tartod itt!
    passwordConfirm?: string // opcionális, élesben általában nem tartod itt!
    registered: string // pl. '2023-11-01'
    avatarUrl?: string // vagy avatar, ha úgy tetszik
}
