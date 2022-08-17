using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data.DataModel.User
{
    public class CustomerDataModel
    {
        [Key]
        public long CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhoneNumber { get; set; }

        public string CustomerAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public ICollection<OrderDataModel> Orders { get; set; }

    }
}
