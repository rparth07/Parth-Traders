using AutoMapper;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Profiles
{
    public class CustomerForAuthProfile : Profile
    {
        public CustomerForAuthProfile()
        {
            CreateMap<CustomerForAuthentication, CustomerForAuthDataModel>();
        }
    }
}
