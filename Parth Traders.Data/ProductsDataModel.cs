using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data
{
    public class ProductsDataModel
    {
        [Key]
        public long ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        public string ProductDescription { get; set; }

        [Required]
        public long SupplierId { get; set; }

        [Required]
        public long CategoryId { get; set; }

        public int QuantityPerUnit { get; set; }

        public long UnitPrice { get; set; }

        public long SingleQuantityMRP { get; set; }

        public long Discount { get; set; }

        public long UnitsInStock { get; set; }


    }
}