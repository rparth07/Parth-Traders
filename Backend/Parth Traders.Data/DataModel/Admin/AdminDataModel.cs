using Microsoft.AspNetCore.Identity;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class AdminDataModel : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
