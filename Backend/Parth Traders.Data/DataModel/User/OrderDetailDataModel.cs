using Parth_Traders.Data.DataModel.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parth_Traders.Data.DataModel.User
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
        public long Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long TotalPrice { get; set; }
    }
}
