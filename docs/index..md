### Key Features

1. **User Authentication**
   - Sign Up / Login / Logout
   - Password Reset

2. **User Profiles**
   - View and Edit Profile Information
   - Upload Profile Picture

3. **Recipe Management**
   - Add New Recipe
   - Edit/Delete Existing Recipe
   - View Recipe Details

4. **Recipe Browsing**
   - Search Recipes
   - Filter Recipes by Ingredients, or Allergenes
   - Sort Recipes by Popularity, Newest, etc.

5. **Recipe Details**
   - Display Recipe Ingredients
   - Step-by-step Instructions
   - Ratings and comments

### Tech Stack

- **Frontend:**
  - HTML, CSS, JavaScript, TypeScript
  - Pinia for state management
  - Vue for a single-page application (SPA)

- **Backend:**
  - ASP.NET Core for the server
  - MySQL for the database
  - JWT for authentication

- **Infrastucture:**
  - Docker for containerization

### Project Structure

#### Frontend
- `src/`
  - `components/`
    - `Auth/` (Login, Signup, etc.)
    - `Profile/` (Profile viewing and editing)
    - `Recipes/` (Recipe list, details, add/edit form)
  - `services/` (API calls)
  - `store/` (State management)
  - `App.ts` (Main app component)
  - `index.ts` (Entry point)

#### Backend
- `controllers/` (Handles requests and responses)
- `models/` (Database schemas)
- `middleware/` (Auth, error handling, etc.)
- `program.cs` (Entry point)

### Development Steps

1. **Setup the Environment**
   - Initialize your project (frontend and backend)
   - Set up version control with Git
   - Install necessary dependencies

2. **Deploy the Application**
   - Choose a hosting platform
   - Set up CI/CD pipelines
   - Deploy frontend and backend

3. **Create User Authentication**
   - Backend: Set up user model, authentication routes, JWT handling
   - Frontend: Create login/signup forms, handle authentication state

4. **Develop User Profiles**
   - Backend: Create user profile routes
   - Frontend: Create profile view and edit components

5. **Implement Recipe Management**
   - Backend: Set up recipe model, routes for adding, editing, deleting recipes
   - Frontend: Create forms for adding and editing recipes, recipe detail view

6. **Add Recipe Browsing and Search**
   - Backend: Implement search and filter routes
   - Frontend: Create components for search and filter, display recipe list


### Sample API Endpoints

#### User Routes
- `GET /api/users/profile` - Get user profile
- `PUT /api/users/profile` - Update user profile

#### Recipe Routes
- `POST /api/recipes` - Add new recipe
- `GET /api/recipes` - Get all recipes
- `GET /api/recipes/:id` - Get a single recipe by ID
- `PUT /api/recipes/:id` - Update a recipe
- `DELETE /api/recipes/:id` - Delete a recipe
