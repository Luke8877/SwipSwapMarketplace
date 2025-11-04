# SwipSwap Marketplace

SwipSwap Marketplace is a Blazor Server application built with .NET 8, Entity Framework Core, and MudBlazor.  
The goal of this project is to create a peer-to-peer marketplace where users can securely post, browse, and purchase products.  
It includes authentication using JWT and a planned integration with Stripe for payments.

---

## Tech Stack

- .NET 8 (Blazor Server)
- Entity Framework Core
- MudBlazor (UI Components)
- JWT Authentication
- Stripe Integration (Planned)

---

## Project Structure

SwipSwapMarketplace/
├── Auth/ → JWT setup and authentication logic
├── Components/ → Blazor UI components (Dashboard, Product, Payment, etc.)
├── Data/ → Database context and configuration
├── Docs/ → Team notes and to-do lists
├── Migrations/ → EF Core migration files
├── Models/ → Entity models (Product, User, Payment, etc.)
├── Services/ → Data and business logic services
├── wwwroot/ → Static assets (CSS, JS, etc.)
└── Program.cs → Application entry point

---

## Team Workflow

### Default Branch: `dev`
All development should be done in the `dev` branch.

### Protected Branch: `main`
The `main` branch is protected and can only be updated through pull requests that are reviewed and approved.

---

## Getting Started 

### 1. Clone the Repository

```bash
git clone https://github.com/Luke8877/SwipSwapMarketplace.git
cd SwipSwapMarketplace
git checkout dev


### 2. Create a Feature Branch

git checkout -b feature/your-feature-name

### 3. Make and Commit Changes

git add .
git commit -m "Add: short description of your change"

### 4. Push Your Branch

git push -u origin feature/your-feature-name

### 5. Open a Pull Request

On GitHub, open a pull request from your feature branch into dev.
All changes must go through pull requests. No one should push directly to main or dev.


## Branch Naming Conventions

| Branch Type | Example | Description |
|--------------|----------|-------------|
| feature | feature/login-page | New functionality |
| fix | fix/payment-service | Bug fixes |
| refactor | refactor/ui-cleanup | Code structure improvements |
| hotfix | hotfix/urgent-fix | Critical patch for production |

---

## Guidelines

- Do not commit directly to `main` or `dev`.
- Always create a new branch for your work.
- Keep pull requests small and focused.
- Write clear and descriptive commit messages.
- Use `git pull` frequently to stay up to date with `dev`.

--- 

**Maintained by the SwipSwap Team — Fall 2025**
