using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models.Shops;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RtuItLab.Infrastructure.Models.Purchases
{
    [ValidateUpdateTransaction]
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
    public class ValidateUpdateTransaction : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (UpdateTransaction)value;
            if (employee.Id <= 0)
                throw new BadRequestException("Invalid Id: Set valid transactionId");
            return ValidationResult.Success;
        }
    }
}
