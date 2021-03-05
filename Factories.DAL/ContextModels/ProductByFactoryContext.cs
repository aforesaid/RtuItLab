using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factories.DAL.ContextModels
{
    public class ProductByFactoryContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FactoryContextKey")]
        public FactoryContext Factory { get; set; }
        public int FactoryContextKey { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int PartsOfCreate { get; set; } 
        public decimal Count { get; set; }
    }
}
