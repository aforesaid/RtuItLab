using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shops.API.Models.ContextModels
{
    public class Shop
    {
        [Key]
        public  int Id { get; set; }
        public string Address { get; set; }
        //TODO: добавить паттерн на телефон
        public string PhoneNumber { get; set; }
        public List<Product> Products { get; set; }
    }
}
