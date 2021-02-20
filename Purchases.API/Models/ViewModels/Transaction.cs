using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ViewModels
{
    public class Transaction
    {
        [Required]
        public List<Product> Products { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public TransactionTypes TransactionType { get; set; } = TransactionTypes.IN_CASH;
        public bool IsUserCreate { get; set; } = true;
        public Receipt Receipt { get; set; }
    }
}
