using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class CustomersDataModel
    {
        [Key]
        public long CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public DateTime CreatedDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public PaymentType PaymentType { get; set; }


    }
}
