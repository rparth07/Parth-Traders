using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class AdminDataModel : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
