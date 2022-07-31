using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class OrdersDataModel
    {
        [Key]
        public long OrderId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long OrderNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime  OrderDate { get; set; }

    }
}
