using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class ProductDataModel
    {
        [Key]
        public long ProductId { get; set; }

        [Required]
        public string ProductSku { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        public string ProductDescription { get; set; }

        public long SupplierId { get; set; }
        [Required]
        [ForeignKey("SupplierId")]
        public virtual SupplierDataModel SupplierData { get; set; }

        public long CategoryId { get; set; }
        [Required]
        [ForeignKey("CategoryId")]
        public virtual CategoryDataModel CategoryData { get; set; }

        public ICollection<OrderDetailDataModel> OrderDetails { get; set; }

        [Required]
        public double ProductRating { get; set; }

        [Required]
        public long UnitPrice { get; set; }

        [Required]
        public int PiecesPerUnit { get; set; }

        [Required]
        public long SinglePieceMRP { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long UnitsInStock { get; set; }

    }
}