using Parth_Traders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Entity
{
    public class Product
    {

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public ProductType ProductType { get; set; }

        public string ProductDescription { get; set; }

        public virtual Supplier SupplierData { get; set; }

        public virtual Category CategoryData { get; set; }

        public int QuantityPerUnit { get; set; }

        public long UnitPrice { get; set; }

        public long SinglePieceMRP { get; set; }

        public long Discount { get; set; }

        public long UnitsInStock { get; set; }
    }
}
