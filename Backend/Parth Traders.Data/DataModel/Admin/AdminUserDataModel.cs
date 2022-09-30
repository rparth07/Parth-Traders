using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class AdminUserDataModel : IdentityUser
    {
        [Key]
        public long AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string AdminName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
