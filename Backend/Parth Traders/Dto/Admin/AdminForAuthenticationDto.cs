using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class AdminForAuthenticationDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}
