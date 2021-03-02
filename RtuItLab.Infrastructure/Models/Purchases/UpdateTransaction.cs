using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.Models.Purchases
{
    public class UpdateTransaction
    {
        [Required]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Product> Products { get; set; }
        [RequireTransactionType]
        public TransactionTypes TransactionType { get; set; }
    }
    public class RequireTransactionType : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (int)value;
            if (employee == 0 || employee == 1)
                return ValidationResult.Success;
            return new ValidationResult("Invalid Transaction Type");
        }
    }
}
