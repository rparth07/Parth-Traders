using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Data.DataModel.Admin
{
    public class CategoryDataModel
    {
        [Key]
        public long CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public ICollection<ProductDataModel> Products { get; set; }
    }
}
