using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class SuppliersDataModel
    {
        [Key]
        public long SupplierID { get; set; }

        [Required]
        public string SupplierName { get; set; }

        public string Address { get; set; }

        public string SupplierEmail { get; set; }

        public string PhoneNumber { get; set; }
    }
}
