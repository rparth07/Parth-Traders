using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderDate = DateTime.Now;
            OrderStatus = OrderStatus.Pending;
        }

        public long OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public long GrandTotal { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }
    }
}
