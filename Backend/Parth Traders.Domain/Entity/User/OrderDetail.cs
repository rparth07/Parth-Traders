using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Domain.Entity.User
{
    public class OrderDetail
    {
        public long OrderDetailId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public decimal Price { get; set; }

        public long Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
