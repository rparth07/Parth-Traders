﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Repositories.Admin
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddCategories(List<Category> categoriesToAdd)
        {
            var categoriesDataToAdd = _mapper.Map<List<CategoryDataModel>>(categoriesToAdd);

            _context.Categories.AddRange(categoriesDataToAdd);
        }

        public void AddCategory(Category categoryToAdd)
        {
            var categoryDataToAdd = _mapper.Map<CategoryDataModel>(categoryToAdd);

            _context.Categories.Add(categoryDataToAdd);
        }

        public void DeleteCategory(Category categoryToRemove)
        {
            var categoryData = _mapper.Map<CategoryDataModel>(categoryToRemove);

            var categoryToDelete = _context.Categories
                .FirstOrDefault(_ => _.CategoryId == categoryData.CategoryId);

            //need to revisit to find should I need to delete orderDetails as,
            //deleting in deleteProduct API and then delete product or directly delete.
            _context.Products.RemoveRange(categoryToDelete.Products);
            _context.Categories.Remove(categoryToDelete);
        }

        public List<Category> GetAllCategories()
        {
            var categories = _context.Categories
                .Include("Products")
                .ToList();

            return _mapper.Map<List<Category>>(categories);
        }

        public Category GetCategory(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(_ => _.CategoryId == id);

            return _mapper.Map<Category>(category);
        }

        public Category GetCategoryByName(string categoryName)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(_ => _.CategoryName == categoryName);

            return _mapper.Map<Category>(category);
        }

        public void UpdateCategory(Category category)
        {
            var categoryDataModel = _mapper.Map<CategoryDataModel>(category);

            var categoryToUpdate = _context.Categories
                .FirstOrDefault(_ => _.CategoryName == category.CategoryName);

            _context.Categories.Update(categoryToUpdate);
            
            Save();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
