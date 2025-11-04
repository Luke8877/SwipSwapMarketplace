using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Data;
using SwipSwapMarketplace.Models;

namespace SwipSwapMarketplace.Services
{
    /// <summary>
    /// Manages all payment-related operations, including database
    /// records and integration points for external payment providers.
    /// </summary>
    public class PaymentService
    {
        private readonly AppDbContext _db;

        public PaymentService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves all payments with related order data
        public async Task<List<Payment>> GetAllAsync()
        {
            return await _db.Payments
                .Include(p => p.Order)
                .ToListAsync();
        }

        // Retrieves a payment by its ID
        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _db.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        // Creates a payment record (called after successful external transaction)
        public async Task AddAsync(Payment payment)
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
        }

        // Updates payment status (e.g., from Pending → Completed)
        public async Task UpdateStatusAsync(int paymentId, string newStatus)
        {
            var payment = await _db.Payments.FindAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = newStatus;
                await _db.SaveChangesAsync();
            }
        }

        // Deletes a payment record if needed
        public async Task DeleteAsync(int id)
        {
            var payment = await _db.Payments.FindAsync(id);
            if (payment != null)
            {
                _db.Payments.Remove(payment);
                await _db.SaveChangesAsync();
            }
        }

        // Placeholder for Stripe (or other API) integration
        public async Task<bool> ProcessExternalPaymentAsync(decimal amount, string providerPaymentId)
        {
            // TODO: Implement real API integration here
            await Task.Delay(100); // simulate network delay
            return true;
        }
    }
}
