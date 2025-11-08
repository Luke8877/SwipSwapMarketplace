using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Models;

namespace SwipSwapMarketplace.Services
{
    /// <summary>
    /// Handles all data operations related to products,
    /// including retrieval, creation, updates, and deletion.
    /// </summary>
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves all products with category and seller info
        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .ToListAsync();

        }

        // Retrieves a single product by ID
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        // Adds a new product to the database
        public async Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        // Updates an existing product
        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        // Deletes a product by its ID
        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }
    }
}
