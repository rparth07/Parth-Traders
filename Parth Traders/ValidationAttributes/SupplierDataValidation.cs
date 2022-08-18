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
            if (supplier.SupplierName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : supplier name should be of minimum 2 characters."
                    , new[] { nameof(SupplierDto) });
            }
            const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (Regex.IsMatch(supplier.SupplierPhoneNumber, motif))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid phone number!"
                    , new[] { nameof(SupplierDto) });
            }
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.
                [0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (regex.IsMatch(supplier.SupplierEmail))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid email address!"
                    , new[] { nameof(SupplierDto) });
            }

            return ValidationResult.Success;
        }
    }
}
