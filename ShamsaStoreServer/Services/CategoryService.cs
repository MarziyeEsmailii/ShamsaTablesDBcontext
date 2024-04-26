using Microsoft.EntityFrameworkCore;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Category?> GetAsync(int id)
        {
            Category? result =
                await _applicationDbContext.Categories.
                FindAsync(id);

            return result;
        }
        public async Task<List<Category>> GetsAsync()
        {
            List<Category> categories =
                await _applicationDbContext.Categories
                .ToListAsync();

            return categories;
        }

        public async Task AddAsync(CategoryCreateViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده خالی است");

            Category category = new Category();

            category.Name = viewModel.Name;

            category.ImageFileName = viewModel.ImageFileName;

            _applicationDbContext.Categories.Add(category);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryEditViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Category? oldCategory =
                await _applicationDbContext.Categories.FindAsync(viewModel.Id);

            if (oldCategory is null)
                throw new Exception("دسته بندی یافت نشد");

            oldCategory.Name = viewModel.Name;

            oldCategory.ImageFileName = viewModel.ImageFileName;

            _applicationDbContext.Categories.Update(oldCategory);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category? category =
                await _applicationDbContext.Categories.FindAsync(id);

            if (category is null)
                throw new Exception("دسته بندی پیدا نشد");

            _applicationDbContext.Categories.Remove(category);

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
