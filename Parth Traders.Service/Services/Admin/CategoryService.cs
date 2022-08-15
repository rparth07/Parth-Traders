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
            _categoryRepository = categoryRepository;
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

        public Category GetCategoryById(long id)
        {
            var categoryToReturn = _categoryRepository.GetCategoryById(id);
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

        public void UpdateCategory(Category category, string oldCategoryName)
        {
            _categoryRepository.UpdateCategory(category, oldCategoryName);
        }
    }
}
