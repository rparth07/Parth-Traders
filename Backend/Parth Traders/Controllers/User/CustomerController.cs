using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Controllers.User
{
    [ApiController]
    [Route("API/user/customers")]
    //[ApiExplorerSettings(GroupName = "v1")]
    public class CustomerController : ControllerBase
    {
        public readonly ICustomerService _customerService;
        public readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService,
                                  IMapper mapper)
        {
            _customerService = customerService ??
                throw new ArgumentNullException(nameof(customerService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddCustomer(CustomerDto customerForCreation)
        {
            var customer = _mapper.Map<Customer>(customerForCreation);

            Customer addedCustomer = _customerService
                .AddCustomer(customer);

            CustomerDto customerToReturn = _mapper.Map<CustomerDto>(addedCustomer);

            return CreatedAtRoute("GetCustomer",
                                  new { userName = customerToReturn.UserName },
                                  customerToReturn);
        }

        [HttpGet("{userName}", Name = "GetCustomer")]
        public IActionResult GetCustomer(string userName)
        {
            CustomerDto customerFromRepo = _mapper.Map<CustomerDto>(_customerService.GetCustomerByUserName(userName));
            return Ok(customerFromRepo);
        }

        [HttpPost("{userName}")]
        [Consumes("application/json")]
        public IActionResult UpdateCustomer(
            string userName,
            CustomerDto customer)
        {
            var customerToUpdate = _mapper.Map<Customer>(customer);

            _customerService.UpdateCustomer(customerToUpdate, userName);

            return Ok(customer);
        }

        [HttpDelete("{userName}")]
        public IActionResult DeleteCustomer(string userName)
        {
            _customerService.DeleteCustomer(userName);
            return NoContent();
        }
    }
}