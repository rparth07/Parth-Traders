using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Controllers
{
    [ApiController]
    [Route("API/admin/products")]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        public readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService ?? 
                throw new ArgumentNullException(nameof(productService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddProduct(ProductDto product)
        {
            Product addedProduct = _productService.AddProduct(_mapper.Map<Product>(product));

            ProductDto productToReturn = _mapper.Map<ProductDto>(addedProduct);

            return CreatedAtRoute("GetProduct",
                new { productName = productToReturn.ProductName },
                productToReturn);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult AddAllProducts()
        {
            IFormFile? file = Request.Form.Files[0];
            //TODO:Need to Add logic to parse file and get list of productsToAdd
            List<ProductDto> productsToAdd = new List<ProductDto>();
            _productService.AddAllProducts(_mapper.Map<List<Product>>(productsToAdd));
            
            return Ok(productsToAdd);
        }

        [HttpGet("{productName}", Name = "GetProduct")]
        public IActionResult GetProduct(string productName)
        {
            ProductDto productFromRepo = _mapper.Map<ProductDto>(_productService.GetProductByProductName(productName));
            return Ok(productFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
            {
            var prodcuts = _mapper.Map<List<ProductDto>>(_productService.GetAllProducts());
            return Ok(prodcuts);
        }

        [HttpDelete("{productName}")]
        public IActionResult DeleteProduct(string productName)
        {
            _productService.DeleteProduct(productName);
            return NoContent();
        }

        [HttpPost("productId")]
        public IActionResult UpdateProduct(
            string productName,
            JsonPatchDocument<ProductDto> patchDocument)
        {
            var productToUpdateFromRepo = _productService.GetProductByProductName(productName);

            var productToPatch = _mapper.Map<ProductDto>(productToUpdateFromRepo);

            patchDocument.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(productToPatch, productToUpdateFromRepo);
            _productService.UpdateProduct(productToUpdateFromRepo);

            return Ok(productToUpdateFromRepo);
        }
    }
}
