using AutoMapper;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<ParsedCustomer, Customer>();
        }
    }
}
