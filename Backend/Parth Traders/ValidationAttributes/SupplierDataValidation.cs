using Parth_Traders.Dto.Admin;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Parth_Traders.ValidationAttributes
{
    public class SupplierDataValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(
                    "Validation Error : Input Value Type is  Invalid , All Value Must Be Valid And In Given constraints."
                    , new[] { nameof(SupplierDto) });
            }
            var supplier = (SupplierDto)validationContext.ObjectInstance;
            if (string.IsNullOrWhiteSpace(supplier.SupplierName) || supplier.SupplierName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : supplier name should be of minimum 2 characters."
                    , new[] { nameof(SupplierDto) });
            }
            Regex motif = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
            if (string.IsNullOrWhiteSpace(supplier.SupplierPhoneNumber) || !motif.IsMatch(supplier.SupplierPhoneNumber))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid phone number!"
                    , new[] { nameof(SupplierDto) });
            }
            if (string.IsNullOrWhiteSpace(supplier.SupplierEmail) || !new EmailAddressAttribute().IsValid(supplier.SupplierEmail))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid email address!"
                    , new[] { nameof(SupplierDto) });
            }

            return ValidationResult.Success;
        }
    }
}
