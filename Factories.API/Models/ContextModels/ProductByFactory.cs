using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factories.API.Models.ContextModels
{
    public class ProductByFactory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FactoryKey")]
        public int FactoryKey { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int TimeForCreate { get; set; } 
    }
}
