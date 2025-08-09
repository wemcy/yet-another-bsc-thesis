# 🍲 Yet Another BSC Thesis – Recipe Management Web Application

## 📖 Leírás
Ez a projekt a szakdolgozatom keretében készülő **receptkezelő webalkalmazás**, amely lehetővé teszi a felhasználók számára receptek digitális rögzítését, böngészését és megosztását.  
A rendszer célja, hogy a receptek könnyen kereshetők és szűrhetők legyenek, különös tekintettel az allergéninformációkra az **1169/2011/EU rendelet** szerint.

## ✨ Funkciók (tervezett)
- Felhasználói regisztráció és bejelentkezés (**Keycloak** alapú autentikáció)
- Új receptek létrehozása, szerkesztése, törlése
- Receptek listázása és részleteinek megtekintése
- Receptek értékelése és kommentelése
- Allergén alapú szűrés és keresés
- Reszponzív, mobilbarát felület

## 🛠 Technológiák
### Frontend
- [Vue 3](https://vuejs.org/) + [Vite](https://vitejs.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Pinia](https://pinia.vuejs.org/) állapotkezelés
- [Tailwind CSS](https://tailwindcss.com/)
- [Vitest](https://vitest.dev/) teszteléshez

### Backend
- [.NET 8](https://dotnet.microsoft.com/) (ASP.NET Core Web API)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) (tervezett)
- [PostgreSQL](https://www.postgresql.org/) adatbázis (tervezett)
- [Swagger/OpenAPI](https://swagger.io/) API dokumentáció

### Autentikáció
- [Keycloak](https://www.keycloak.org/) – OAuth2 / OpenID Connect

### DevOps
- [Docker](https://www.docker.com/) + [Docker Compose](https://docs.docker.com/compose/)
- GitHub Container Registry
- GitVersion verziókövetés
