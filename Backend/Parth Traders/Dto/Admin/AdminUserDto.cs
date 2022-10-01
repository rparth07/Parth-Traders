using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class AdminUserDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
