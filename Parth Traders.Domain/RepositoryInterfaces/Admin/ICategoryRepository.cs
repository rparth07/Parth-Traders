﻿using Parth_Traders.Domain.Entity;
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
        void AddAllCategories(List<Category> categoriesToAdd);
        Category GetCategoryById(long id);
        Category GetCategoryByName(string categoryName);
        List<Category> GetAllCategories();
        void UpdateCategory(Category category, string oldCategoryName);
        void DeleteCategory(Category categoryToRemove);
        bool Save();
        void Dispose();
    }
}
