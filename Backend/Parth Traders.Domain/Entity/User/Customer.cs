using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.Entity.User
{
    public class Customer
    {
        public Customer()
        {
            CreatedDate = DateTime.Now;
        }

        public string CustomerId { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CustomerAddress { get; set; }

        public PaymentType PaymentType { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
