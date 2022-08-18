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
            if (customer.CustomerName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : customer name should be of minimum 2 characters."
                    , new[] { nameof(CustomerDto) });
            }
            const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (Regex.IsMatch(customer.CustomerPhoneNumber, motif))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid phone number!"
                    , new[] { nameof(CustomerDto) });
            }
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.
                [0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (regex.IsMatch(customer.CustomerEmail))
            {
                return new ValidationResult(
                    "Validation Error : Please enter a valid email address!"
                    , new[] { nameof(CustomerDto) });
            }

            return ValidationResult.Success;
        }
    }
}
