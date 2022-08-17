using Parth_Traders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Entity
{
    public class Order
    {
        public long OrderId { get; set; }

        public virtual Customer Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime OrderDate { get; set; }

        public long GrandTotal { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
