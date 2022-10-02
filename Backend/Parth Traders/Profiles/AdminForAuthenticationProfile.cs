using AutoMapper;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Profiles
{
    public class AdminForAuthenticationProfile : Profile
    {
        public AdminForAuthenticationProfile()
        {
            CreateMap<AdminForAuthenticationDto, AdminForAuthentication>();
        }
    }
}
