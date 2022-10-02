using AutoMapper;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdminForRegistrationDto, AdminDataModel>();
        }
    }
}
