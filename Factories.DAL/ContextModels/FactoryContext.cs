using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factories.DAL.ContextModels
{
    public class FactoryContext
    {
        [Key]
        public int Id { get; set; }
        public List<ProductByFactoryContext> Products { get; set; }
    }
}
