using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchases.DAL.ContextModels
{
    public class ProductContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("TransactionContextKey")]
        public int TransactionContextKey { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; } = 1;
        public string Category { get; set; }
    }
}
