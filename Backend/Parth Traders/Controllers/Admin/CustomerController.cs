using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Controllers.Admin
{
    [ApiController]
    [Route("API/admin/customers"), Authorize]
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

        /*        [HttpPost]
                [Consumes("application/json")]
                public IActionResult AddCustomer(CustomerDto customerForCreation)
                {
                    var customer = _mapper.Map<Customer>(customerForCreation);

                    Customer addedCustomer = _customerService
                        .AddCustomer(customer);

                    CustomerDto customerToReturn = _mapper.Map<CustomerDto>(addedCustomer);

                    return CreatedAtRoute("GetCustomer",
                                          new { userName = customerToReturn.userName },
                                          customerToReturn);
                }*/

        //TODO: 1.Admin API - for case where admin need to add list of customer.
        //                    for this case admin will use below API. But need 
        //                    to change API route for below API.
        /*        [HttpPost, DisableRequestSizeLimit]
                public IActionResult AddAllCustomers([FromForm] IFormFile file)
                {
                    List<ParsedCustomer> parsedcustomers = new ParsedCustomer().ParseData(file);

                    var customersToAdd = _mapper.Map<List<Customer>>(parsedcustomers);

                    var addedcustomers = _customerService.AddAllCustomers(customersToAdd);

                    return Ok(_mapper.Map<List<CustomerDto>>(addedcustomers));
                }
        */
        [HttpGet("{userName}", Name = "GetCustomerInfo")]
        public IActionResult GetCustomerInfo(string userName)
        {
            CustomerDto customerFromRepo = _mapper.Map<CustomerDto>(_customerService.GetCustomerByUserName(userName));
            return Ok(customerFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerService.GetAllCustomers());
            return Ok(customers);
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
