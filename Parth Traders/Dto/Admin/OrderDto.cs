using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrderDto
    {
        [Required]
        public virtual CustomerDto CustomerData { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        [Required]
        public int PaymentType { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public long GrandTotal { get; set; }
    }
}
