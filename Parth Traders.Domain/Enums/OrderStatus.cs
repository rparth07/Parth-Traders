using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Approved,
        Processing,
        CanceledByCustomer,
        CanceledByOwner,
        Completed
    }
}
