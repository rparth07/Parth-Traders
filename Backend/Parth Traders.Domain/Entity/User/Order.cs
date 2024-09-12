using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.Entity.User
{
    public class Order
    {
        public long OrderId { get; set; }

        public string TransactionId { get; set; }

        public virtual Customer Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime OrderDate { get; set; }

        public long GrandTotal { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
