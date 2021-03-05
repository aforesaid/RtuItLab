using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shops.DAL.ContextModels
{
    public class ProductContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ShopContextKey")]
        public ShopContext Shop { get; set; }
        public int ShopContextKey { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
    }
}
