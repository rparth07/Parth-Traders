using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.User
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            CreatedDate = DateTime.Now;
            Orders = new List<OrderDto>();
        }

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

        public ICollection<OrderDto> Orders { get; set; }

    }
}
