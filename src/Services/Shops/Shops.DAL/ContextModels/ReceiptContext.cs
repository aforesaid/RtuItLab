using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shops.DAL.ContextModels
{
    public class ReceiptContext 
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ShopContextKey")]
        public ShopContext ShopContext { get; set; }
        public int ShopContextKey { get; set; }
        public List<ProductByReceiptContext> Products { get; set; }
        public decimal FullCost { get; set; }
        public int Count { get; set; }
    }
}
