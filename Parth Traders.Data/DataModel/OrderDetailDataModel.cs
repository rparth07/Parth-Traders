﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.DataModel
{
    public class OrderDetailDataModel
    {
        [Key]
        public long OrderDetailId { get; set; }

        public long OrderId { get; set; }
        [Required]
        [ForeignKey("OrderId")]
        public virtual OrderDataModel OrderData { get; set; }

        public long ProductId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public virtual ProductDataModel ProductData { get; set; } 

        [Required]
        public long PricePerPiece { get; set; }

        [Required]
        public long QuantityPurchased { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public DateTime BillDate { get; set; }  
    }
}
