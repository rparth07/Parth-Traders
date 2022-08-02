﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.DataModel
{
    public class OrderDetailsDataModel
    {
        [Key]
        public long OrderDetailId { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public virtual OrdersDataModel OrdersData { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public virtual ProductsDataModel ProductsData { get; set; } 

        [Required]
        public long Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public DateTime BillDate { get; set; }  
    }
}
