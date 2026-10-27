using AutoMapper;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Data.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDataModel>();
            CreateMap<CustomerDataModel, Customer>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
