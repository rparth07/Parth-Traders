using AutoMapper;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForPath(dest => dest.Supplier.SupplierName,
                    opt => opt.MapFrom(src => src.SupplierName))
                .ForPath
                (dest => dest.Category.CategoryName,
                    opt => opt.MapFrom(src => src.CategoryName));

            CreateMap<ParsedProduct, Product>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.SupplierName,
                    opt => opt.MapFrom(src => src.Supplier.SupplierName))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.OrderDetails,
                    opt => opt.MapFrom(src => src.OrderDetails));
        }
    }
}
