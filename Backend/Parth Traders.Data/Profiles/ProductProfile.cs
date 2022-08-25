using AutoMapper;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDataModel>()
                .ForMember(dest => dest.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier.SupplierId))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.Category.CategoryId));

            CreateMap<ProductDataModel, Product>()
                .ForMember(dest => dest.Supplier,
                    opt => opt.MapFrom(src => src.SupplierData))
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.CategoryData));
        }
    }
}
