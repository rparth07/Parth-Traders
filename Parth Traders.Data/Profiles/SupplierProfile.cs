using AutoMapper;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;

namespace Parth_Traders.Data.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDataModel>();
            CreateMap<SupplierDataModel, Supplier>();
        }
    }
}
