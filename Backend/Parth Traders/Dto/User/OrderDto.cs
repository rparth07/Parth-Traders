using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.User
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderDate = DateTime.Now;
            OrderStatus = OrderStatus.Pending;
        }

        public long OrderId { get; set; }

        public string TransactionId { get; set; }

        [Required]
        public string UserName { get; set; }

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
