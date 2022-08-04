using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Entity
{
    public class Supplier
    {
        public long SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierAddress { get; set; }

        public string SupplierEmail { get; set; }

        public string SupplierPhoneNumber { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
