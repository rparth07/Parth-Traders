using AutoMapper;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierDto, Supplier>();
        }
    }
}
