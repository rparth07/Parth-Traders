using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    public class CategoryDto
    {
        public CategoryDto()
        {
            Products = new List<ProductDto>();
        }

        [Required]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public ICollection<ProductDto> Products { get; set; }

    }
}
