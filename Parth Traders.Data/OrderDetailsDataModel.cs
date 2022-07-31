using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class OrderDetailsDataModel
    {
        [Key]
        public long OrderDetailId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        public long ProductId { get; set; } 

        public long OrderNumber { get; set; }

        public long Price { get; set; }

        public long Quantity { get; set; }

        public long Discount { get; set; }

        public long Total { get; set; }

        public DateTime BillDate { get; set; }  
    }
}
