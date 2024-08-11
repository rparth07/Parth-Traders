using AutoMapper;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;

namespace Parth_Traders.Profiles
{
    public class CustomerForAuthProfile : Profile
    {
        public CustomerForAuthProfile()
        {
            CreateMap<CustomerForAuthDto, CustomerForAuthentication>();
            CreateMap<CustomerDataModel, CustomerForAuthentication>();
        }
    }
}
