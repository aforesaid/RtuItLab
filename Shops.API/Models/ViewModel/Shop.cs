using System.Collections.Generic;

namespace Shops.API.Models.ViewModel
{
    public class Shop
    { 
        public int Id { get; set; }
        public string Address { get; set; }
        //TODO: добавить паттерн на телефон
        public string PhoneNumber { get; set; }
        public List<Product> Products { get; set; }

    }
}
