﻿using AutoMapper;
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
    [Route("API/admin/products"), Authorize]
    //[ApiExplorerSettings(GroupName = "v2")]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        private readonly IProductHelperService _productHelperService;
        public readonly IMapper _mapper;
        public ProductController(IProductService productService, IProductHelperService productHelperService, IMapper mapper)
        {
            _productHelperService = productHelperService ??
                throw new ArgumentException(nameof(productHelperService));
            _productService = productService ??
                throw new ArgumentNullException(nameof(productService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("add-product")]
        [Consumes("application/json")]
        public IActionResult AddProduct(ProductDto productForCreation)
        {
            var product = _mapper.Map<Product>(productForCreation);

            var productToAdd = _productHelperService.MapProductPropertiesToProduct(product);

            Product addedProduct = _productService
                .AddProduct(productToAdd);

            ProductDto productToReturn = _mapper.Map<ProductDto>(addedProduct);

            return CreatedAtRoute("GetProduct",
                new { productName = productToReturn.ProductName },
                productToReturn);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult AddAllProducts()
        {
            IFormFile? file = Request.Form.Files[0];
            List<ParsedProduct> parsedProducts = new ParsedProduct().ParseData(file);

            List<Product> productsToAdd = parsedProducts.Select(parsedProduct =>
            {
                return _productHelperService
                    .MapProductPropertiesToProduct(_mapper.Map<Product>(parsedProduct));
            }).ToList();

            var addedProducts = _productService.AddAllProducts(productsToAdd);

            return Ok(_mapper.Map<List<ProductDto>>(addedProducts));
        }

        [HttpGet("{productName}", Name = "GetProduct")]
        public IActionResult GetProduct(string productName)
        {
            ProductDto productFromRepo = _mapper.Map<ProductDto>(_productService.GetProductByName(productName));
            return Ok(productFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productService.GetAllProducts());
            return Ok(products);
        }

        [HttpPost("{productName}")]
        [Consumes("application/json")]
        public IActionResult UpdateProduct(
            string productName,
            ProductDto product)
        {
            var productToUpdate = _mapper.Map<Product>(product);

            _productService.UpdateProduct(productToUpdate, productName);

            return Ok(product);
        }

        [HttpDelete("{productName}")]
        public IActionResult DeleteProduct(string productName)
        {
            _productService.DeleteProduct(productName);
            return NoContent();
        }
    }
}
