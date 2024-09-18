using Microsoft.AspNetCore.Identity;
using Parth_Traders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data.DataModel.User
{
    public class CustomerDataModel : IdentityUser
    {
        public CustomerDataModel(): base()
        {
            
        }
        //TODO: need to verify
        public string CustomerAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public ICollection<OrderDataModel> Orders { get; set; }

    }
}
