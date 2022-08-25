using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class SupplierDataModel
    {
        [Key]
        public long SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; }

        public string SupplierAddress { get; set; }

        public string SupplierEmail { get; set; }

        [Required]
        public string SupplierPhoneNumber { get; set; }

        public ICollection<ProductDataModel> Products { get; set; }
    }
}
