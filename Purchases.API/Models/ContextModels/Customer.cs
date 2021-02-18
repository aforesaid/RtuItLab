using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ContextModels
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
