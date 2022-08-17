using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface ICategoryRepository
    {
        void AddCategory(Category categoryToAdd);
        void AddAllCategories(List<Category> categoriesToAdd);
        Category GetCategoryById(long categoryId);
        Category GetCategoryByName(string categoryName);
        List<Category> GetAllCategories();
        void UpdateCategory(Category category);
        void DeleteCategory(Category categoryToRemove);
        bool Save();
        void Dispose();
    }
}
