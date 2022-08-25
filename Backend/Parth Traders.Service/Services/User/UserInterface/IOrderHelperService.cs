using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Service.Services.User.UserInterface
{
    public interface IOrderHelperService
    {
        Order MapOrderPropertiesToOrder(Order order);
    }
}
