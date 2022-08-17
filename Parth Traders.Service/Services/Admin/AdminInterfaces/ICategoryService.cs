using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface ICategoryService
    {
        Category AddCategory(Category categoryToAdd);
        List<Category> AddAllCategories(List<Category> categoriesToAdd);
        Category GetCategoryById(long categoryId);
        Category GetCategoryByName(string categoryName);
        List<Category> GetAllCategories();
        void UpdateCategory(Category updatedCategory, string oldCategoryName);
        void DeleteCategory(string categoryName);
    }
}
