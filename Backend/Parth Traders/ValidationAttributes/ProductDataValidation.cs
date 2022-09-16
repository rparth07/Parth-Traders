using Parth_Traders.Domain.Enums;
using Parth_Traders.Dto.Admin;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.ValidationAttributes
{
    public class ProductDataValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(
                    "Validation Error : Input Value Type is  Invalid , All Value Must Be Valid And In Given constraints."
                    , new[] { nameof(ProductDto) });
            }
            var product = (ProductDto)validationContext.ObjectInstance;
            if (product.ProductName.Length < 2)
            {
                return new ValidationResult(
                    "Validation Error : Name should be of minimum 2 characters."
                    , new[] { nameof(ProductDto) });
            }

            if (!Enum.IsDefined(typeof(ProductType), product.ProductType))
            {
                return new ValidationResult(
                    "Validation Error : Product type should be one of the following Buff, Machine_tools, General, Packing_materials."
                    , new[] { nameof(ProductDto) });
            }

            if (product.PiecesPerUnit < 0)
            {
                return new ValidationResult(
                    "Validation Error : Pieces per unit can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            if (product.SinglePieceMRP < 0)
            {
                return new ValidationResult(
                    "Validation Error : Single piece MRP can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            if (product.UnitPrice < 0)
            {
                return new ValidationResult(
                    "Validation Error : Unit price can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            if (product.Discount < 0)
            {
                return new ValidationResult(
                    "Validation Error : Discount can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            if (product.UnitsInStock < 0)
            {
                return new ValidationResult(
                    "Validation Error : Units in stocks can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            if (product.UnitPrice != product.PiecesPerUnit * product.SinglePieceMRP)
            {
                return new ValidationResult(
                    "Validation Error : Unit price does not match with total price of all individual pieces in 1 unit."
                    , new[] { nameof(ProductDto) });
            }

            return ValidationResult.Success;
        }
    }
}
