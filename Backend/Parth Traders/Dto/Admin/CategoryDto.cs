using Parth_Traders.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Parth_Traders.Dto.Admin
{
    [CategoryDataValidation]
    public class CategoryDto
    {
        public CategoryDto()
        {
            Products = new List<ProductDto>();
        }

        public long CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public ICollection<ProductDto> Products { get; set; }

    }
}
