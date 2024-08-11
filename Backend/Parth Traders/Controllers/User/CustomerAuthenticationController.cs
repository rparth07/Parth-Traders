using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [Consumes("application/json")]
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
            } catch (Exception ex)
            {

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
            return Ok(new
            {
                Token = await _authManager.CreateToken(),
                Customer = _cutomerManager.Users
                                        .FirstOrDefault(_ => _.Email == customer.Email),
                Success = true
            });
        }

//TODO: need to verify
        [HttpPost("customer-details/{customerId}")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync(string customerId,
                                                                 [FromBody] CustomerDataModel customerDetailsToUpdate)
        {
            var customer = await _cutomerManager.FindByIdAsync(customerId);
            customer.UserName = customerDetailsToUpdate.UserName;
            customer.Email = customerDetailsToUpdate.Email;
            customer.PhoneNumber = customer.PhoneNumber;
            customer.CustomerAddress = customerDetailsToUpdate.CustomerAddress; //confirm this

            await _cutomerManager.UpdateAsync(customer);
            CustomerForAuthDto customerDto = new CustomerForAuthDto()
            {
                Email = customer.Email,
                Password = customer.PasswordHash
            };
            return await Authenticate(customerDto);
        }

//TODO: need to verify
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
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
