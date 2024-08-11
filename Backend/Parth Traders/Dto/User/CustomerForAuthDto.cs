using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.User
{
    public class CustomerForAuthDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}
