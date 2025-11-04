using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Auth;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Models;
using System.Security.Cryptography;
using System.Text;

namespace SwipSwapMarketplace.Services
{
    /// <summary>
    /// Provides user authentication and registration logic for the system.
    /// This service interacts with the database and uses <see cref="JwtService"/> to issue tokens.
    /// </summary>
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="context">Database context for user data.</param>
        /// <param name="jwtService">JWT service used for token creation.</param>
        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Validates user credentials and generates a JWT token upon successful authentication.
        /// </summary>
        /// <param name="email">User's email address.</param>
        /// <param name="password">User's plaintext password.</param>
        /// <returns>The JWT token if credentials are valid; otherwise, <c>null</c>.</returns>
        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return _jwtService.GenerateToken(user.UserId.ToString(), user.Username);
        }

        /// <summary>
        /// Registers a new user in the system if the email is not already taken.
        /// </summary>
        /// <param name="username">Desired username.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="password">User's plaintext password.</param>
        /// <returns><c>true</c> if registration is successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return false;

            var hashedPassword = HashPassword(password);
            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Hashes a plaintext password using SHA256.
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Verifies that a provided plaintext password matches a stored hashed password.
        /// </summary>
        private bool VerifyPassword(string password, string storedHash)
        {
            var hashed = HashPassword(password);
            return hashed == storedHash;
        }
    }
}
