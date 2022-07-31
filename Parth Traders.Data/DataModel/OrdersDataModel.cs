using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.DataModel
{
    public class OrdersDataModel
    {
        [Key]
        public long OrderId { get; set; }

        [Required]
        public virtual CustomersDataModel CustomerData { get; set; }
        
        [Required]
        public PaymentType PaymentType { get; set; }
        
        [Required]
        public DateTime  OrderDate { get; set; }

    }
}
