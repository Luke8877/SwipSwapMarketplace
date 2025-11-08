using SwipSwapMarketplace.Views;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Text;
using SwipSwapMarketplace.Services;

//
// Entry point for the SwipSwap Marketplace application.
// Configures services, authentication, middleware, and database initialization for the Blazor Server app.
//

var builder = WebApplication.CreateBuilder(args);

#region Service Configuration

// Register Razor components and enable interactive server rendering
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register the application's database context with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register MudBlazor component library services
builder.Services.AddMudServices();

//
// JWT Authentication Setup
//
// Configure authentication services using JWT Bearer tokens. 
// The configuration values (key, issuer, audience, and expiration) are defined in appsettings.json.
//
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//
// Register application services
//
builder.Services.AddScoped<JwtService>();     // Handles JWT creation
builder.Services.AddScoped<AuthService>();    // Handles login and registration logic
builder.Services.AddScoped<ProductService>();   // Provides CRUD operations for products
builder.Services.AddScoped<PaymentService>();   // Handles payment records and Stripe integration

#endregion

var app = builder.Build();

#region Middleware Configuration

// Configure global error handling and security for non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Standard middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

//
// Authentication and Authorization Middleware
//
// These must be placed after static files and before endpoint mappings
// to ensure secure routes and token validation are enforced.
//
app.UseAuthentication();
app.UseAuthorization();

// Map the root Razor component for rendering
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

#endregion

#region Database Initialization

// Create and seed the database on startup if it does not exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Add an initial demo user if the Users table is empty
    if (!db.Users.Any())
    {
        db.Users.Add(new SwipSwapMarketplace.Models.User
        {
            Username = "Jane",
            Email = "Jane@example.com",
            PasswordHash = "demo123"
        });
        db.SaveChanges();
    }
}

#endregion

app.Run();
