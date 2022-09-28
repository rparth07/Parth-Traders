using Parth_Traders.Domain.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateAdmin(AdminAuthentication adminAuth);
        Task<string> CreateToken();
    }
}
