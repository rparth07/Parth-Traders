using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.CsvParserModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;
using Parth_Traders.Helper;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Controllers
{
    [ApiController]
    [Route("API/admin/categories")]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;
        public readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService,
                                  IMapper mapper)
        {
            _categoryService = categoryService ??
                throw new ArgumentNullException(nameof(categoryService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddCategory(CategoryDto categoryForCreation)
        {
            var category = _mapper.Map<Category>(categoryForCreation);

            Category addedCategory = _categoryService
                .AddCategory(category);

            CategoryDto categoryToReturn = _mapper.Map<CategoryDto>(addedCategory);

            return CreatedAtRoute("GetCategory",
                                  new { categoryName = categoryToReturn.CategoryName },
                                  categoryToReturn);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult AddAllCategories()
        {
            IFormFile? file = Request.Form.Files[0];
            //TODO:Need to Add logic to parse file and get list of categoriesToAdd
            List<ParsedCategory> parsedcategories = new ParsedCategory().ParseData(file);

            var categoriesToAdd = _mapper.Map<List<Category>>(parsedcategories);

            var addedcategories = _categoryService.AddAllCategories(categoriesToAdd);

            return Ok(_mapper.Map<List<CategoryDto>>(addedcategories));
        }

        [HttpGet("{categoryName}", Name = "GetCategory")]
        public IActionResult GetCategory(string categoryName)
        {
            CategoryDto categoryFromRepo = _mapper.Map<CategoryDto>(_categoryService.GetCategoryByName(categoryName));
            return Ok(categoryFromRepo);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryService.GetAllCategories());
            return Ok(categories);
        }

        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory(string categoryName)
        {
            _categoryService.DeleteCategory(categoryName);
            return NoContent();
        }

        [HttpPost("{categoryName}")]
        [Consumes("application/json")]
        public IActionResult UpdateCategory(
            string categoryName,
            CategoryDto category)
        {
            var categoryToUpdate = _mapper.Map<Category>(category);

            _categoryService.UpdateCategory(categoryToUpdate, categoryName);

            return Ok(category);
        }
    }
}
