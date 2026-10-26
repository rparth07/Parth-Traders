using Parth_Traders.Dto.User;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Parth_Traders.ValidationAttributes
{
    public class CustomerDataValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(
                    "Validation Error : Input Value Type is  Invalid , All Value Must Be Valid And In Given constraints."
                    , new[] { nameof(CustomerDto) });
            }
            var customer = (CustomerDto)validationContext.ObjectInstance;
            if (string.IsNullOrWhiteSpace(customer.UserName) || customer.UserName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : customer name should be of minimum 2 characters."
                    , new[] { nameof(CustomerDto) });
            }
            // Adjusted regex pattern to allow phone numbers without delimiters
            const string motif = @"^(\(?[0-9]{3}\)?[-. ]?[0-9]{3}[-. ]?[0-9]{4}|\d{10})$";
            if (string.IsNullOrWhiteSpace(customer.PhoneNumber) || !Regex.IsMatch(customer.PhoneNumber, motif))
            {
                return new ValidationResult(
                    "Validation Error: Please enter a valid phone number!",
                    new[] { nameof(CustomerDto) });
            }
            if (string.IsNullOrWhiteSpace(customer.Email) || !new EmailAddressAttribute().IsValid(customer.Email))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid email address!"
                    , new[] { nameof(CustomerDto) });
            }

            return ValidationResult.Success;
        }
    }
}
