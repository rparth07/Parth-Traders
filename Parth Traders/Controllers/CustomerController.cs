﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;
using Parth_Traders.Helper;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Controllers
{
    [ApiController]
    [Route("API/admin/customers")]
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
                                  new { customerName = customerToReturn.CustomerName },
                                  customerToReturn);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult AddAllCustomers()
        {
            IFormFile? file = Request.Form.Files[0];
            //TODO:Need to Add logic to parse file and get list of customersToAdd
            List<ParsedCustomer> parsedcustomers = new ParsedCustomer().ParseData(file);

            var customersToAdd = _mapper.Map<List<Customer>>(parsedcustomers);

            var addedcustomers = _customerService.AddAllCustomers(customersToAdd);

            return Ok(_mapper.Map<List<CustomerDto>>(addedcustomers));
        }

        [HttpGet("{customerName}", Name = "GetCustomer")]
        public IActionResult GetCustomer(string customerName)
        {
            CustomerDto customerFromRepo = _mapper.Map<CustomerDto>(_customerService.GetCustomerByName(customerName));
            return Ok(customerFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerService.GetAllCustomers());
            return Ok(customers);
        }

        [HttpDelete("{customerName}")]
        public IActionResult DeleteCustomer(string customerName)
        {
            _customerService.DeleteCustomer(customerName);
            return NoContent();
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
    }
}
