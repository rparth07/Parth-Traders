using AutoMapper;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>()
                .ForPath(dest => dest.SupplierData.SupplierName,
                    opt => opt.MapFrom(src => src.SupplierName))
                .ForPath
                (dest => dest.CategoryData.CategoryName,
                    opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}
