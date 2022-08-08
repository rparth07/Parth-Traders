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

        public ICollection<OrderDetailDataModel> OrderDetails { get; set; }    
        
        [Required]
        public PaymentTypeDataModel PaymentType { get; set; }
        
        [Required]
        public DateTime  OrderDate { get; set; }

    }
}
