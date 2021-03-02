using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RtuItLab.Infrastructure.Models.Shops
{
    public class Shop
    {
        public int Id { get; set; }
        public string Address { get; set; }
        [RegularExpression("[7-9][0-9]{9}")]
        public string PhoneNumber { get; set; }
        public List<Product> Products { get; set; }

    }
}
