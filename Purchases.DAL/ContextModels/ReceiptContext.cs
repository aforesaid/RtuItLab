using System;
using System.ComponentModel.DataAnnotations;

namespace Purchases.DAL.ContextModels
{
    public class ReceiptContext
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ShopId { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public int TransactionId { get; set; }

        public TransactionContext Transaction { get; set; }
    }
}
