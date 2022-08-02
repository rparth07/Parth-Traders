using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class SupplierDto
    {
        [Required]
        public string SupplierName { get; set; }

        public string SupplierAddress { get; set; }

        public string SupplierEmail { get; set; }

        [Required]
        public string SupplierPhoneNumber { get; set; }

        public ICollection<ProductsDto> Products { get; set; }

    }
}
