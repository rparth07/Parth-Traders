using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface ICategoryService
    {
        Category AddCategory(Category categoryToAdd);
        List<Category> AddAllCategories(List<Category> categoriesToAdd);
        Category GetCategoryById(long id);
        Category GetCategoryByName(string categoryName);
        List<Category> GetAllCategories();
        void UpdateCategory(Category category, string oldCategoryName);
        void DeleteCategory(string categoryName);
    }
}
