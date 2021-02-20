using System;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ContextModels
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
    }
}
