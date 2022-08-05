using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Controllers
{
    [ApiController]
    [Route("API/Admin/Product")]
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

        [HttpGet("{productName}", Name = "GetProduct")]
        public IActionResult GetProduct(string productName)
        {
            ProductDto productFromRepo = _mapper.Map<ProductDto>(_productService.GetProductByProductName(productName));
            return Ok(productFromRepo);
        }

    }
}
