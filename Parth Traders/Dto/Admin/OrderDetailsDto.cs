using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrderDetailsDto
    {
        [Required]
        public virtual OrdersDto OrdersData { get; set; }

        [Required]
        public virtual ProductsDto ProductsData { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public DateTime BillDate { get; set; }
    }
}
