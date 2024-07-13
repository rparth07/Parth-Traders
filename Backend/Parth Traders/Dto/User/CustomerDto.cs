using Parth_Traders.Domain.Enums;
using Parth_Traders.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.User
{
    [CustomerDataValidation]
    public class CustomerDto
    {
        public CustomerDto()
        {
            Orders = new List<OrderDto>();
        }

        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string CustomerAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public ICollection<OrderDto> Orders { get; set; }

    }
}
