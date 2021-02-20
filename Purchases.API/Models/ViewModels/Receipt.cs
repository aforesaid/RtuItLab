using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ViewModels
{
    public class Receipt
    {
        [Required]
        public List<Product> Products { get; set; }
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
