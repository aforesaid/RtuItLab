using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServicesDtoModels.Models.Shops;

namespace ServicesDtoModels.Models.Purchases
{
    [RequireWhenIsShop]
    public class Transaction
    {
        public List<Product> Products { get; set; }

        public DateTime Date { get; set; }

        public TransactionTypes TransactionType { get; set; } = TransactionTypes.InCash;
        public bool IsShopCreate { get; set; }

        public Receipt Receipt { get; set; }
    }
    //TODO: проверить, работает ли
    public class RequireWhenIsShopAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Transaction)value;
            if (employee.Products != null || employee.Products?.Count == 0)
                return new ValidationResult("Invalid Products: Products can't be null");
            if (!employee.IsShopCreate)
                return ValidationResult.Success;
            if (employee.Receipt != null ||
                employee.Receipt?.ShopId != default || employee.Receipt?)
            return IsValid(employee.Receipt) ? ValidationResult.Success : new ValidationResult("Invalid Receipt value");
        }
    }
}
