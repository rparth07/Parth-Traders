using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface ICategoryRepository
    {
        void AddCategory(Category categoryToAdd);
        void AddCategories(List<Category> categoriesToAdd);
        Category GetCategory(int id);
        Category GetCategoryByName(string categoryName);
        List<Category> GetAllCategories();
        void UpdateCategory(Category categoryToUpdate);
        void DeleteCategory(Category categoryToRemove);
    }
}
