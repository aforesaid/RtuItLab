using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.Models.Purchases
{
    public class Receipt
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<Product> Products { get; set; }
    }
}
