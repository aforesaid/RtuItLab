using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shops.DAL.ContextModels
{
    public class ShopContext
    {
        [Key]
        public  int Id { get; set; }
        public string Address { get; set; }
        //TODO: добавить паттерн на телефон
        public string PhoneNumber { get; set; }
        public List<ProductContext> Products { get; set; }
    }
}
