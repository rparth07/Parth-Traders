using AutoMapper;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierDto, Supplier>();
            CreateMap<ParsedSupplier, Supplier>();
            CreateMap<Supplier, SupplierDto>();
        }
    }
}
