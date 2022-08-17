using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.Entity.User
{
    public class Customer
    {
        public long CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string CustomerAddress { get; set; }

        public PaymentType PaymentType { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
