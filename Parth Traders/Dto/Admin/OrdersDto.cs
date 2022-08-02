using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrdersDto
    {
        [Required]
        public virtual CustomersDto CustomerData { get; set; }

        public ICollection<OrderDetailsDto> OrderDetails { get; set; }

        [Required]
        public int PaymentType { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
    }
}
