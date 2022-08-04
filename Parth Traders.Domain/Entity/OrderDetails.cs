using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Entity
{
    public class OrderDetails
    {
        public long OrderDetailId { get; set; }

        public virtual Order OrdersData { get; set; }

        public virtual Product ProductsData { get; set; }

        public long Price { get; set; }

        public long Quantity { get; set; }

        public long Discount { get; set; }

        public long Total { get; set; }

        public DateTime BillDate { get; set; }
    }
}
