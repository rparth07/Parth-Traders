using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.User
{
    public class OrderDetailDto
    {
        public OrderDetailDto()
        {
        }

        public long OrderDetailId { get; set; }

        public long OrderId { get; set; }

        [Required]
        public string ProductSku { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
