using Microsoft.EntityFrameworkCore;
using Npgsql;
using Parth_Traders.ValidationAttributes;
using AutoMapper;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Parth_Traders.Data.DataModel.User;

using Parth_Traders.Domain.Entity.User;

using Parth_Traders.Domain.RepositoryInterfaces.User;

using Parth_Traders.Dto.User;

namespace Parth_Traders.Controllers.User

{

    [Route("api/user/authentication")]

    [ApiController]

    public class CustomerAuthenticationController : ControllerBase

    {

        private readonly UserManager<CustomerDataModel> _cutomerManager;

        private readonly ICustomerAuthManager _authManager;

        private readonly IMapper _mapper;

        public CustomerAuthenticationController(IMapper mapper,

                                        UserManager<CustomerDataModel> customerManager,

                                        ICustomerAuthManager authManager)

        {

            _mapper = mapper;

            _cutomerManager = customerManager;

            _authManager = authManager;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterUser(CustomerDto customerDto)
        {
            var phone = RegistrationPhoneAttribute.Normalize(customerDto.PhoneNumber);
            if (phone == null) return RegistrationErrors(400, new() { ["phone"] = new[] { RegistrationPhoneAttribute.Guidance } });
            try
            {
                var conflicts = new Dictionary<string, string[]>();
                if (await _cutomerManager.FindByNameAsync(customerDto.UserName) != null)
                    conflicts["username"] = new[] { "This username is already in use. Choose another username." };
                if (await _cutomerManager.FindByEmailAsync(customerDto.Email) != null)
                    conflicts["email"] = new[] { "An account already uses this email address. Sign in, or use another email." };
                if (await _cutomerManager.Users.AnyAsync(c => c.PhoneNumber == phone))
                    conflicts["phone"] = new[] { "An account already uses this phone number. Sign in, or use another number." };
                if (conflicts.Count > 0) return RegistrationErrors(409, conflicts);

                var customerUser = _mapper.Map<CustomerDataModel>(customerDto);
                customerUser.PhoneNumber = phone;
                var result = await _cutomerManager.CreateAsync(customerUser, customerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = new Dictionary<string, List<string>>();
                    foreach (var error in result.Errors)
                    {
                        var (field, message) = error.Code switch
                        {
                            "DuplicateUserName" => ("username", "This username is already in use. Choose another username."),
                            "InvalidUserName" => ("username", "Use only letters, numbers, dots, underscores, @, + or hyphens. Spaces are not allowed."),
                            "DuplicateEmail" => ("email", "An account already uses this email address. Sign in, or use another email."),
                            "InvalidEmail" => ("email", "Enter a valid email address."),
                            "PasswordTooShort" => ("password", $"Use at least {_cutomerManager.Options.Password.RequiredLength} characters in your password."),
                            "PasswordRequiresDigit" => ("password", "Include at least one number in your password."),
                            "PasswordRequiresUpper" => ("password", "Include an uppercase letter in your password."),
                            "PasswordRequiresLower" => ("password", "Include a lowercase letter in your password."),
                            "PasswordRequiresNonAlphanumeric" => ("password", "Include a symbol in your password."),
                            _ => ("form", "These account details were not accepted. Review your entries and try again.")
                        };
                        if (!errors.ContainsKey(field)) errors[field] = new();
                        errors[field].Add(message);
                    }
                    return RegistrationErrors(400, errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));
                }
                return StatusCode(201);
            }
            catch (DbUpdateException exception) when (exception.GetBaseException() is PostgresException { SqlState: "23505" })
            {
                // A concurrent registration can race the checks above. Never expose SQL details or submitted values.
                var constraint = ((PostgresException)exception.GetBaseException()).ConstraintName ?? "";
                var field = constraint.Contains("PhoneNumber", StringComparison.OrdinalIgnoreCase) ? "phone"
                    : constraint.Contains("Email", StringComparison.OrdinalIgnoreCase) ? "email" : "username";
                return RegistrationErrors(409, new() { [field] = new[] { $"An account already uses this {field}. Sign in, or choose another." } });
            }
            catch (Exception)
            {
                return StatusCode(503, new { message = "Registration is temporarily unavailable. Your details have not been rejected; please try again later." });
            }
        }

        private ObjectResult RegistrationErrors(int status, Dictionary<string, string[]> errors) =>
            StatusCode(status, new { message = "Please correct the highlighted fields.", errors });

        [HttpPost("login")]

        public async Task<IActionResult> Authenticate([FromBody] CustomerForAuthDto customer)

        {

            if (!await _authManager.ValidateCustomer(_mapper.Map<CustomerForAuthentication>(customer)))

            {

                return Unauthorized();

            }

            var newlyAddedCustomer = _cutomerManager.Users

                                        .FirstOrDefault(_ => _.Email == customer.Email);

            return Ok(new

            {

                Token = await _authManager.CreateToken(),

                Customer = new { newlyAddedCustomer.Id, newlyAddedCustomer.UserName, newlyAddedCustomer.Email, newlyAddedCustomer.PhoneNumber, newlyAddedCustomer.CustomerAddress, newlyAddedCustomer.PaymentType },

                Success = true

            });

        }

//TODO: need to verify

        public sealed record CustomerProfileUpdate(string? Email, string? PhoneNumber, string? CustomerAddress);
        [HttpPost("customer-details/{customerId}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync(string customerId, [FromBody] CustomerProfileUpdate profile)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != customerId) return Forbid();
            var customer = await _cutomerManager.FindByIdAsync(customerId);
            if (customer == null) return NotFound();
            customer.Email = profile.Email ?? customer.Email;
            customer.PhoneNumber = profile.PhoneNumber ?? customer.PhoneNumber;
            customer.CustomerAddress = profile.CustomerAddress ?? customer.CustomerAddress;
            var result = await _cutomerManager.UpdateAsync(customer);
            if (!result.Succeeded) return BadRequest(new { message = "Profile could not be updated. Check the email and phone number." });
            return Ok(new { Customer = new { customer.Id, customer.UserName, customer.Email, customer.PhoneNumber, customer.CustomerAddress, customer.PaymentType } });
        }

        [HttpDelete("{customerId}"), Authorize(Roles = "Customer")]

        public async Task<IActionResult> DeleteCustomer(string customerId)

        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != customerId) return Forbid();
            var customer = await _cutomerManager.FindByIdAsync(customerId);

            if (customer != null)

            {

                IdentityResult result = await _cutomerManager.DeleteAsync(customer);

                if (!result.Succeeded)

                {

                    foreach (var error in result.Errors)

                    {

                        ModelState.TryAddModelError(error.Code, error.Description);

                    }

                    return BadRequest(ModelState);

                }

            }

            else

            {

                return BadRequest("No customer found!");

            }

            return Ok("customer deleted successfully.");

        }

    }

}
