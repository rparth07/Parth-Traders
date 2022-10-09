using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Dto.Admin;
using Parth_Traders.Helper;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Controllers.Admin
{
    [ApiController]
    [Route("API/admin/suppliers"), Authorize]
    public class SupplierController : ControllerBase
    {
        public readonly ISupplierService _supplierService;
        public readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService,
                                  IMapper mapper)
        {
            _supplierService = supplierService ??
                throw new ArgumentNullException(nameof(supplierService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("add-supplier")]
        [Consumes("application/json")]
        public IActionResult AddSupplier(SupplierDto supplierForCreation)
        {
            var supplier = _mapper.Map<Supplier>(supplierForCreation);

            Supplier addedSupplier = _supplierService
                .AddSupplier(supplier);

            SupplierDto supplierToReturn = _mapper.Map<SupplierDto>(addedSupplier);

            return CreatedAtRoute("GetSupplier",
                                  new { supplierName = supplierToReturn.SupplierName },
                                  supplierToReturn);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult AddAllSuppliers()
        {
            IFormFile? file = Request.Form.Files[0];
            List<ParsedSupplier> parsedsuppliers = new ParsedSupplier().ParseData(file);

            var suppliersToAdd = _mapper.Map<List<Supplier>>(parsedsuppliers);

            var addedsuppliers = _supplierService.AddAllSuppliers(suppliersToAdd);

            return Ok(_mapper.Map<List<SupplierDto>>(addedsuppliers));
        }

        [HttpGet("{supplierName}", Name = "GetSupplier")]
        public IActionResult GetSupplier(string supplierName)
        {
            SupplierDto supplierFromRepo = _mapper.Map<SupplierDto>(_supplierService.GetSupplierByName(supplierName));
            return Ok(supplierFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _mapper.Map<List<SupplierDto>>(_supplierService.GetAllSuppliers());
            return Ok(suppliers);
        }

        [HttpPost("{supplierName}")]
        [Consumes("application/json")]
        public IActionResult UpdateSupplier(
            string supplierName,
            SupplierDto supplier)
        {
            var supplierToUpdate = _mapper.Map<Supplier>(supplier);

            _supplierService.UpdateSupplier(supplierToUpdate, supplierName);

            return Ok(supplier);
        }

        [HttpDelete("{supplierName}")]
        public IActionResult DeleteSupplier(string supplierName)
        {
            _supplierService.DeleteSupplier(supplierName);
            return NoContent();
        }
    }
}
