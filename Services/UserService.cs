using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Models;

namespace SwipSwapMarketplace.Services
{
    /// <summary>
    /// Handles all database operations related to user accounts,
    /// including retrieval, profile management, and association with products or orders.
    /// </summary>
    public class UserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves all users
        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users
                .Include(u => u.Products)
                .Include(u => u.Orders)
                .ToListAsync();
        }

        // Retrieves a user by ID, including related data
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users
                .Include(u => u.Products)
                .Include(u => u.Orders)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        // Adds a new user (basic insert)
        public async Task AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        // Updates existing user details
        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        // Deletes a user (with related cascade behavior handled by EF)
        public async Task DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }
    }
}
