using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Service.Services.Admin
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ??
                throw new ArgumentNullException(nameof(categoryRepository));
        }

        public Category AddCategory(Category categoryToAdd)
        {
            try
            {
                _categoryRepository.AddCategory(categoryToAdd);
                _categoryRepository.Save();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetCategoryByName(categoryToAdd.CategoryName);
        }

        public List<Category> AddAllCategories(List<Category> categoriesToAdd)
        {
            try
            {
                _categoryRepository.AddAllCategories(categoriesToAdd);
                _categoryRepository.Save();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("One or more category data is in wrong format. " +
                    "Please enter data in correct format!");
            }

            List<Category> addedCategories = categoriesToAdd
                .Select(_ => GetCategoryByName(_.CategoryName))
                .ToList();

            return addedCategories;
        }

        public Category GetCategoryById(long categoryId)
        {
            var categoryToReturn = _categoryRepository.GetCategoryById(categoryId);
            if (categoryToReturn == null)
            {
                throw new NotFoundException("Please enter a valid Id!");
            }
            return categoryToReturn;
        }

        public Category GetCategoryByName(string categoryName)
        {
            var categoryToReturn = _categoryRepository.GetCategoryByName(categoryName);
            if (categoryToReturn == null)
            {
                throw new NotFoundException("Please enter a valid category name!");
            }
            return categoryToReturn;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        public void UpdateCategory(Category updatedCategory, string oldCategoryName)
        {
            var categoryFromRepo = GetCategoryByName(oldCategoryName);

            _categoryRepository
                .UpdateCategory(FillRequiredInfo(categoryFromRepo, updatedCategory));
        }

        private static Category FillRequiredInfo(Category categoryFromRepo,
                                                 Category category)
        {
            category.CategoryId = categoryFromRepo.CategoryId;
            category.Products = categoryFromRepo.Products;

            return category;
        }

        public void DeleteCategory(string categoryName)
        {
            try
            {
                var categoryFromRepo = GetCategoryByName(categoryName);

                _categoryRepository.DeleteCategory(categoryFromRepo);
                _categoryRepository.Save();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Please enter a valid category name!");
            }
        }
    }
}
