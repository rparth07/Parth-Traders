using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parth_Traders.Data.DataModel.User
{
    public class OrderDataModel
    {
        [Key]
        public long OrderId { get; set; }

        public string TransactionId { get; set; }

        public long CustomerId { get; set; }
        [Required]
        [ForeignKey("Id")]
        public virtual CustomerDataModel CustomerData { get; set; }

        [Required]
        public ICollection<OrderDetailDataModel> OrderDetails { get; set; }

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
