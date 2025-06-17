export interface User {
    name: string
    email: string
    password?: string // opcionális, élesben általában nem tartod itt!
    registered: string // pl. '2023-11-01'
    // később jöhet még pl.: avatarUrl, id, stb.
}
