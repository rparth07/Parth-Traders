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
                                  new { customerName = customerToReturn.UserName },
                                  customerToReturn);
        }

        [HttpGet("{customerName}", Name = "GetCustomer")]
        public IActionResult GetCustomer(string customerName)
        {
            CustomerDto customerFromRepo = _mapper.Map<CustomerDto>(_customerService.GetCustomerByName(customerName));
            return Ok(customerFromRepo);
        }

        [HttpPost("{customerName}")]
        [Consumes("application/json")]
        public IActionResult UpdateCustomer(
            string customerName,
            CustomerDto customer)
        {
            var customerToUpdate = _mapper.Map<Customer>(customer);

            _customerService.UpdateCustomer(customerToUpdate, customerName);

            return Ok(customer);
        }

        [HttpDelete("{customerName}")]
        public IActionResult DeleteCustomer(string customerName)
        {
            _customerService.DeleteCustomer(customerName);
            return NoContent();
        }
    }
}