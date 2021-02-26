using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServicesDtoModels.Models.Shops;

namespace ServicesDtoModels.Models.Purchases
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
