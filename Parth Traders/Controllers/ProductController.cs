using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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


    }
}
