using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrderDetailDto
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string ProductName { get; set; }

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
