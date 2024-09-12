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
        public long PricePerPiece { get; set; }

        [Required]
        public long QuantityPurchased { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Total { get; set; }
    }
}
