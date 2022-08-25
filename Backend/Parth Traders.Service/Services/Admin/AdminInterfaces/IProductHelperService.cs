using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface IProductHelperService
    {
        Product MapProductPropertiesToProduct(Product product);
    }
}
