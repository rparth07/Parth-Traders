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
                    "Validation Error : product name should be of minimum 2 characters."
                    , new[] { nameof(ProductDto) });
            }

            if (!Enum.IsDefined(typeof(ProductType), product.ProductType))
            {
                return new ValidationResult(
                    "Validation Error : product type should be one of the following Buff, Machine_tools, General, Packing_materials."
                    , new[] { nameof(ProductDto) });
            }

            if (product.UnitPrice != product.PiecesPerUnit * product.SinglePieceMRP)
            {
                return new ValidationResult(
                    "Validation Error : unit price does not match with total individual price of pieces in 1 unit."
                    , new[] { nameof(ProductDto) });
            }

            if (product.UnitPrice < 0 ||
                product.PiecesPerUnit < 0 ||
                product.SinglePieceMRP < 0 ||
                product.Discount < 0 ||
                product.UnitsInStock < 0)
            {
                return new ValidationResult(
                    "Validation Error : unit price, pieces per unit, single piece MRP, discount and units in stocks can not be less than zero."
                    , new[] { nameof(ProductDto) });
            }

            return ValidationResult.Success;
        }
    }
}
