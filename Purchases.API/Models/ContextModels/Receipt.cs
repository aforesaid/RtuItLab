using System;
using System.ComponentModel.DataAnnotations;

namespace Purchases.API.Models.ContextModels
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public int IdShop { get; set; }
        public int IdStuff { get; set; }
        public string NameStuff { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
