using Parth_Traders.Domain.Enums;
using Parth_Traders.Dto.User;
using Parth_Traders.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    [ProductDataValidation]
    public class ProductDto
    {
        public ProductDto()
        {
            OrderDetails = new List<OrderDetailDto>();
        }

        public long ProductId { get; set; }

        public string ProductSku { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        public string ProductDescription { get; set; }

        public double ProductRating { get; set; }

        public string? SupplierName { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        [Required]
        public int PiecesPerUnit { get; set; }

        [Required]
        public decimal SinglePieceMRP { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public long UnitsInStock { get; set; }
    }
}
