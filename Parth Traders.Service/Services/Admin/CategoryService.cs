using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Category> AddAllCategories(List<Category> categoriesToAdd)
        {
            try
            {
                _categoryRepository.AddAllCategories(categoriesToAdd);
                _categoryRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Bad Request {ex}");
            }

            return categoriesToAdd;
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
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return categoryToAdd;
        }

        public void DeleteCategory(string categoryName)
        {
            try
            {
                var categoryFromRepo = _categoryRepository.GetCategoryByName(categoryName);
                _categoryRepository.DeleteCategory(categoryFromRepo);
                _categoryRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Category not found!");
            }
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        public Category GetCategoryById(long categoryId)
        {
            var categoryToReturn = _categoryRepository.GetCategoryById(categoryId);
            if (categoryToReturn == null)
            {
                throw new Exception("Category not found!");
            }
            return categoryToReturn;
        }

        public Category GetCategoryByName(string categoryName)
        {
            var categoryToReturn = _categoryRepository.GetCategoryByName(categoryName);
            if (categoryToReturn == null)
            {
                throw new Exception("Category not found!");
            }
            return categoryToReturn;
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
    }
}
