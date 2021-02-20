using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ViewModels
{
    public class UpdateTransaction
    {
        [Required]
        public  int Id { get; set; }
        public  DateTime Date { get; set; }
        public List<Product> Products { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
