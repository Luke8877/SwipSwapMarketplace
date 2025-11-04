using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Models;

namespace SwipSwapMarketplace.Services
{
    /// <summary>
    /// Provides functionality for managing customer orders,
    /// including retrieval, creation, and status updates.
    /// </summary>
    public class OrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves all orders with buyer, product, and payment details
        public async Task<List<Order>> GetAllAsync()
        {
            return await _db.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .Include(o => o.Payment)
                .ToListAsync();
        }

        // Retrieves a specific order by ID
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _db.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        // Adds a new order
        public async Task AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        // Updates order details (status, delivery type, etc.)
        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        // Deletes an order
        public async Task DeleteAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }
    }
}
