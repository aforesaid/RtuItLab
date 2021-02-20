using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Purchases.API.Models.ViewModels;

namespace Purchases.API.Models.ContextModels
{
    public class TransactionContext 
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerKey")]
        public int CustomerKey { get; set; }
        public List<ProductContext> Products { get; set; }
        public DateTime Date { get; set; }
        public TransactionTypes TransactionType { get; set; } = TransactionTypes.IN_CASH;
        public bool IsUserCreate { get; set; }
        public ReceiptContext Receipt { get; set; }

    }
}
