using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class ProductDto
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductType { get; set; }

        public string ProductDescription { get; set; }

        [Required]
        public virtual SupplierDto SupplierData { get; set; }

        [Required]
        public virtual CategoryDto CategoryData { get; set; }

        public ICollection<OrderDetailsDto> OrderDetails { get; set; }

        [Required]
        public int QuantityPerUnit { get; set; }

        [Required]
        public long UnitPrice { get; set; }

        [Required]
        public long SinglePieceMRP { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long UnitsInStock { get; set; }
    }
}
