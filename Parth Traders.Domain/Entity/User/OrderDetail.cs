using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Domain.Entity.User
{
    public class OrderDetail
    {
        public long OrderDetailId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public long PricePerPiece { get; set; }

        public long QuantityPurchased { get; set; }

        public long Discount { get; set; }

        public long Total { get; set; }

        public DateTime BillDate { get; set; }
    }
}
