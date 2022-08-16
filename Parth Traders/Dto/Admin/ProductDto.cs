using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parth_Traders.Dto.Admin
{
    public class ProductDto
    {
        public ProductDto()
        {
            OrderDetails = new List<OrderDetailDto>();
        }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductType { get; set; }

        public string ProductDescription { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        [Required]
        public int PiecesPerUnit { get; set; }

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
