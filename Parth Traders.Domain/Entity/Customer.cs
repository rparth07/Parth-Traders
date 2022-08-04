using Parth_Traders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Entity
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
