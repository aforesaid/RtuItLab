using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.Models.Purchases
{
    public class UpdateTransaction
    {
        [Required]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Product> Products { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
