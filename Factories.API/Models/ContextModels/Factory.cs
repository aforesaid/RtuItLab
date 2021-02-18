using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factories.API.Models.ContextModels
{
    public class Factory
    {
        [Key]
        public int Id { get; set; }
        public List<ProductByFactory> Products { get; set; }
    }
}
