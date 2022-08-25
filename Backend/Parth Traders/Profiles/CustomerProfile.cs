using AutoMapper;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;

namespace Parth_Traders.Data.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<ParsedCustomer, Customer>();
            CreateMap<Customer, CustomerDto>();
        }
    }
}
