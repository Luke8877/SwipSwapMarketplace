using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwipSwapMarketplace.Models
{
    /// <summary>
    /// Represents a payment record linked to an order.
    /// Used to track transactions processed through Stripe or other payment providers.
    /// </summary>
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        /// <summary>
        /// Foreign key referencing the related order.
        /// Each payment belongs to exactly one order.
        /// </summary>
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        /// <summary>
        /// The payment provider used (e.g., Stripe, PayPal, etc.).
        /// </summary>
        [Required]
        public string Provider { get; set; } = "Stripe";

        /// <summary>
        /// The unique identifier from the payment provider (e.g., Stripe PaymentIntent ID).
        /// Used to verify and manage transactions externally.
        /// </summary>
        public string? ProviderPaymentId { get; set; }

        /// <summary>
        /// Current payment status (e.g., Pending, Succeeded, Failed, Refunded).
        /// </summary>
        [Required]
        public string PaymentStatus { get; set; } = "Pending";

        /// <summary>
        /// Timestamp when the payment was created or confirmed.
        /// </summary>
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Total amount charged for this payment.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Navigation property for the related order.
        /// Defines a one-to-one relationship where Order is the principal entity.
        /// </summary>
        public Order? Order { get; set; }
    }
}
