using AutoMapper;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;

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
