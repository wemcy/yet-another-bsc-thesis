# System Diagrams

This page collects high-level diagrams for the recipe application based on the documented use cases in this repository and the current implementation in `src/`.

## Use Case Diagram

The diagram below summarizes the main user-facing and administrator-facing interactions currently described in the use case documents.

```mermaid
flowchart LR
    user[User]
    admin[Administrator]

    admin -.-> user

    subgraph system[Recipe Application]
        uc1([Register Account])
        uc2([Add New Recipe])
        uc3([Search Recipes])
        uc4([View Recipe Details])
        uc5([Rate Recipe])
        uc6([Add Comment])
        uc7([Edit Recipe])
        uc8([Delete Recipe])
        uc9([View User Profile])
        uc10([Update User Profile])
        uc11([Delete Comment])
        uc12([Manage Ingredients])
    end

    user --- uc1
    user --- uc2
    user --- uc3
    user --- uc4
    user --- uc5
    user --- uc6
    user --- uc7
    user --- uc8
    user --- uc9
    user --- uc10

    admin --- uc11
    admin --- uc12

    uc3 -. leads to .-> uc4
    uc4 -. includes .-> uc5
    uc4 -. includes .-> uc6
    uc9 -. extends to .-> uc10
```

### Notes

- `Administrator` is modeled as a specialized `User`.
- Rating and commenting happen from the recipe details page, so they are shown as related to `View Recipe Details`.
- `Delete Comment` is restricted to administrators according to [011.md](./011.md).
- Ingredient administration is documented in [012.md](./012.md).

## Architecture Diagram

The diagram below reflects the implemented runtime architecture described in [README.md](../../README.md) and visible in `src/frontend`, `src/backend`, and `src/proxy`.

```mermaid
flowchart LR
    browser[Browser<br/>User / Administrator]

    subgraph docker[Docker Compose Runtime]
        proxy[Nginx Reverse Proxy<br/>src/proxy]

        subgraph frontend[Frontend Container]
            ui[Vue 3 + Vite Web App<br/>Views, Router, Pinia]
            apiClient[Generated TypeScript API Client<br/>recipe-api-client]
        end

        subgraph backend[Backend Container]
            controllers[ASP.NET Core Controllers<br/>Auth, Profile, Recipe, Ingredients]
            services[Application Services<br/>AuthService, RecipeService, UserService,<br/>IngredientSuggestionService, ImageService]
            repos[Repository Layer<br/>RecipeRepository, ImageRepository,<br/>IngredientSuggestionRepository, ShowcaseRepository]
            security[Identity, Cookies, Authorization Handlers]
            hosted[Background Service<br/>ShowcaseRefreshService]
        end

        db[(PostgreSQL 17<br/>recipe_database)]
        storage[(Recipe Image Storage<br/>Docker Volume)]
    end

    browser -->|HTTP/HTTPS| proxy
    proxy -->|/| ui
    proxy -->|/api| controllers

    ui --> apiClient
    apiClient -->|REST + cookies| proxy

    controllers --> services
    controllers --> security
    services --> repos
    services --> storage
    repos --> db
    hosted --> repos
    hosted --> db
```

### Architecture Notes

- The proxy is the single public entry point and routes `/` to the frontend and `/api` to the backend.
- Authentication uses ASP.NET Core Identity with cookie-based sessions.
- The backend persists application data in PostgreSQL and stores uploaded recipe images in a mounted volume.
- The frontend uses the generated API client to call the backend through the proxy rather than calling the backend container directly.
