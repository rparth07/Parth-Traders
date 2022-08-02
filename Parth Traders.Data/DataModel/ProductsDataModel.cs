using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parth_Traders.Data.DataModel
{
    public class ProductsDataModel
    {
        [Key]
        public long ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public ProductTypeDataModel ProductType { get; set; }

        public string ProductDescription { get; set; }

        [Required]
        public virtual SuppliersDataModel SupplierData { get; set; }

        [Required]
        public virtual CategoryDataModel CategoryData { get; set; }

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