# SwipSwap Marketplace – Project Setup Summary

### Project Overview
This project uses **Blazor Server (.NET 8)** with **Entity Framework Core** and **MudBlazor** for UI components.  
JWT authentication will be implemented for login, and Stripe will handle payments.

---

### Current Setup
- **Models**: All core entities created and commented for clarity.
- **AppDbContext**: Configured relationships and migration applied.
- **Auth Folder**: Contains `JwtSettings`, `JwtService`, and `AuthService` (for token handling).
- **Services**: Product, User, Payment, and Order logic separated for clean DI.
- **Components**:
  - `Auth` → Login and signup
  - `Dashboard` → Main item browsing page
  - `Product` → Product details with Google Maps placeholder
  - `Payment` → Payment form placeholder
- **Layout**: Global UI structure with `MainLayout.razor` and `NavMenu.razor`.

---

### Notes for Team

- Each main feature (**Login**, **Dashboard**, **Product Details**, **Payment**) has its own folder under `Components/`.
  - Example: all files related to login go in `Components/Auth/`, and payment-related UI goes in `Components/Payment/`.

- The `Services` folder contains the logic that talks to the database (like getting products, users, etc.).
  - When you need to load or save data, call one of these service methods instead of writing new database code directly.

- We’re not using MVC-style controllers — Blazor pages handle their own logic using these services.

- Keep new files or shared UI elements clean and organized.
  - If something could be reused (like a button or card layout), it can later go in a `Shared` folder under `Components/`.

---

### Next Steps
1. Complete AuthService JWT integration 
2. Implement CRUD for Products 
3. Connect PaymentService with Stripe sandbox
4. Add interactive map to ProductDetails
5. Hook up navigation routes in NavMenu
