using AutoMapper;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Profiles
{
    public class AdminForAuthenticationProfile : Profile
    {
        public AdminForAuthenticationProfile()
        {
            CreateMap<AdminForAuthentication, AdminForAuthenticationDataModel>();
        }
    }
}
