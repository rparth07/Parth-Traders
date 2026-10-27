using Parth_Traders.Domain.Enums;
using Parth_Traders.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
namespace Parth_Traders.Dto.User;

public class CustomerDto
{
    [Required(ErrorMessage = "Choose a username.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Use a username between 2 and 100 characters.")]
    [RegularExpression(@"^[A-Za-z0-9._@+\-]+$", ErrorMessage = "Use only letters, numbers, dots, underscores, @, + or hyphens in your username. Spaces are not allowed.")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Enter a password.")]
    [MinLength(10, ErrorMessage = "Use at least 10 characters in your password.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Enter your email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(254, ErrorMessage = "Email addresses must not exceed 254 characters.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Enter your phone number.")]
    [RegistrationPhone]
    public string PhoneNumber { get; set; } = "";

    [Required(ErrorMessage = "Enter your address.")]
    [StringLength(500, ErrorMessage = "Use no more than 500 characters for your address.")]
    public string CustomerAddress { get; set; } = "";
    public PaymentType PaymentType { get; set; }
    public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
}
