using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchases.API.Models.ContextModels
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerKey")]
        public int CustomerKey { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public bool IsUserCreate { get; set; }
        public string Category { get; set; }
        public Receipt Receipt { get; set; } 
        
    }
}
