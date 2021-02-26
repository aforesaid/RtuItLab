using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServicesDtoModels.Models.Shop;

namespace ServicesDtoModels.Models.Purchases
{
    public class Transaction
    {
        [Required]
        public List<Product> Products { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public TransactionTypes TransactionType { get; set; } = TransactionTypes.InCash;
        public bool IsShopCreate { get; set; }

        [RequireWhenIsShop]
        public Receipt Receipt { get; set; }
    }
    //TODO: проверить, работает ли
    public class RequireWhenIsShopAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Transaction)value;
            if (employee.IsShopCreate)
                return ValidationResult.Success;
            return IsValid(employee.Receipt) ? ValidationResult.Success : new ValidationResult("Invalid Receipt value");
        }
    }
}
