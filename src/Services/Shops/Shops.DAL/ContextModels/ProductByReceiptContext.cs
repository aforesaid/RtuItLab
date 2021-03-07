using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shops.DAL.ContextModels
{
    public class ProductByReceiptContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ReceiptContextKey")]
        public ReceiptContext Receipt { get; set; }
        public int ReceiptContextKey { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
    }
}
