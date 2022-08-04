using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrderDto
    {
        [Required]
        public virtual CustomerDto CustomerData { get; set; }

        public ICollection<OrderDetailsDto> OrderDetails { get; set; }

        [Required]
        public int PaymentType { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
    }
}
