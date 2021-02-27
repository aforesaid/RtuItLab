using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shops.DAL.ContextModels
{
    public class ProductContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ShopKey")]
        public int ShopKey { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
