using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class CategoryDataModel
    {
        [Key]
        public long CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }
    }
}
