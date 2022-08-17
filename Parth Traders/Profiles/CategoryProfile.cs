using AutoMapper;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<ParsedCategory, Category>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
