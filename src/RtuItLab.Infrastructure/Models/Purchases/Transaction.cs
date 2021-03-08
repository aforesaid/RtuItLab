using Newtonsoft.Json;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models.Shops;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RtuItLab.Infrastructure.Models.Purchases
{
    [RequireWhenIsShop]
    public class Transaction
    {
        public int Id { get; set; }
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<Product> Products { get; set; }

        public DateTime Date { get; set; }
        public TransactionTypes TransactionType { get; set; } 
        public bool IsShopCreate { get; set; }

        public Receipt Receipt { get; set; }
    }
    public class RequireWhenIsShopAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Transaction)value;
            if (employee.Products == null || employee.Products?.Count == 0)
                throw new BadRequestException("Invalid Products: Products can't be null");
            if (!employee.IsShopCreate)
            {
                if (employee.Receipt != null)
                    throw new BadRequestException(@"Receipt must be null! Use ""receipt"":null in your request");

                return ValidationResult.Success;
            }

            if (employee.Receipt == null || employee.Receipt.Cost == default || employee.Receipt.ShopId == default)
                throw new BadRequestException("Invalid Receipt value");
            return ValidationResult.Success;
        }
    }
}
