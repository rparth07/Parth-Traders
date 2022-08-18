using Parth_Traders.Dto.Admin;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.ValidationAttributes
{
    public class CategoryDataValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(
                    "Validation Error : Input Value Type is  Invalid , All Value Must Be Valid And In Given constraints."
                    , new[] { nameof(CategoryDto) });
            }
            var category = (CategoryDto)validationContext.ObjectInstance;
            if (category.CategoryName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : category name should be of minimum 2 characters."
                    , new[] { nameof(CategoryDto) });
            }

            return ValidationResult.Success;
        }
    }
}
