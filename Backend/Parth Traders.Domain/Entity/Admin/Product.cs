using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.Entity.Admin
{
    public class Product
    {
        public Product()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public long ProductId { get; set; }

        public string ProductSku { get; set; }

        public int ProductRating { get; set; }

        public string ProductName { get; set; }

        public ProductType ProductType { get; set; }

        public string ProductDescription { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public int PiecesPerUnit { get; set; }

        public long UnitPrice { get; set; }

        public long SinglePieceMRP { get; set; }

        public long Discount { get; set; }

        public long UnitsInStock { get; set; }
    }
}
