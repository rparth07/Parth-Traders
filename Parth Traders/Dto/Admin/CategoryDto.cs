using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class CategoryDto
    {
        [Required]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public ICollection<ProductDto> Products { get; set; }

    }
}
