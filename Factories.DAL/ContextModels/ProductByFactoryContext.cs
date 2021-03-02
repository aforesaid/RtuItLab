using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factories.DAL.ContextModels
{
    public class ProductByFactoryContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FactoryKey")]
        public int FactoryKey { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int PartsOfCreate { get; set; } 
        public decimal Count { get; set; }
    }
}
