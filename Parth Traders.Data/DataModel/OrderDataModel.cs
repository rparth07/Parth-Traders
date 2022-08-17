using Parth_Traders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.DataModel
{
    public class OrderDataModel
    {
        [Key]
        public long OrderId { get; set; }

        public long CustomerId { get; set; }
        [Required]
        [ForeignKey("CustomerId")]
        public virtual CustomerDataModel CustomerData { get; set; }

        [Required]
        public ICollection<OrderDetailDataModel> OrderDetails { get; set; }    
        
        [Required]
        public PaymentType PaymentType { get; set; }
        
        [Required]
        public DateTime  OrderDate { get; set; }

        [Required]
        public long GrandTotal { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }
    }
}
