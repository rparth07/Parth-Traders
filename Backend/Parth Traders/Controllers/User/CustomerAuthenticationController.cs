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

        //[Consumes("application/json")]

        public async Task<IActionResult> RegisterUser(CustomerDto customerDto)

        {

            var customerUser = _mapper.Map<CustomerDataModel>(customerDto);

            try

            {

                var result = await _cutomerManager.CreateAsync(customerUser, customerDto.Password);

                if (!result.Succeeded)

                {

                    foreach (var error in result.Errors)

                    {

                        ModelState.TryAddModelError(error.Code, error.Description);

                    }

                    return BadRequest(ModelState);

                }

            } catch (Exception)

            {

                return StatusCode(503, new { message = "Registration is temporarily unavailable. Please try again later." });

            }

            return StatusCode(201);

        }

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
