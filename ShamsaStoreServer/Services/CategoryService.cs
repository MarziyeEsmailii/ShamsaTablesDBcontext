using Microsoft.AspNetCore.Mvc;
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

        #region واکشی اطلاعات دسته بندی محصولات با توجه به آیدی
        public async Task<Category?> GetAsync(int id)
        {
            Category? result =
                await _applicationDbContext.Categories.
                FindAsync(id);

            return result;
        }
        #endregion

        #region واکشی کلی جدول دسته بندی
        public async Task<List<Category>> GetsAsync()
        {
            List<Category> categories =
                await _applicationDbContext.Categories
                .ToListAsync();

            return categories;
        }
        #endregion

        #region اضافه کردن داده به جدول دسته بندی با توحه به اطلاعات مدل دریافتی
        public async Task AddAsync(CategoryDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده خالی است");

            Category category = new Category();

            category.Name = model.Name;
            category.ImageFileName = model.ImageFileName;

            _applicationDbContext.Categories.Add(category);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region ویرایش جدول دسته بندی
        public async Task EditAsync( CategoryDto model)
        {
            //چک کردن خالی نبودن مدل ارسالی
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Category? oldCategory =
                await _applicationDbContext.Categories.FindAsync(model.Id);

            if (oldCategory is null)
                throw new Exception("دسته بندی یافت نشد");

            oldCategory.Name = model.Name;
            oldCategory.ImageFileName = model.ImageFileName;

            _applicationDbContext.Categories.Update(oldCategory);
            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region حذف دسته بندی با استفاده از آیدی
        public async Task DeleteAsync(int id)
        {
            Category? category =
                await _applicationDbContext.Categories.FindAsync(id);

            if (category is null)
                throw new Exception("دسته بندی پیدا نشد");

            _applicationDbContext.Categories.Remove(category);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
