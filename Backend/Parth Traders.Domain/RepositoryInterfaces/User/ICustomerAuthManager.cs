using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface ICustomerAuthManager
    {
        Task<bool> ValidateCustomer(Entity.User.CustomerForAuthentication customerAuth);
        Task<string> CreateToken();
    }
}
