using AutoMapper;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDataModel>();
            CreateMap<CategoryDataModel, Category>();
        }
    }
}
