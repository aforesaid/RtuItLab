using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shops.DAL.ContextModels
{
    public class ShopContext
    {
        [Key]
        public  int Id { get; set; }
        public string Address { get; set; }
        [RegularExpression("[7-9][0-9]{9}")]
        public string PhoneNumber { get; set; }
        public List<ProductContext> Products { get; set; }
    }
}
