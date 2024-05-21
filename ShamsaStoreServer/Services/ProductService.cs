using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        #region اضافه کردن محصول جدید
        /// <summary>
        /// یک محصول جدید  به پایگاه داده اضافه می‌کند.
        /// </summary>
        /// <param name="model">تفاصيل محصولی که باید اضافه شود.</param>
        /// <exception cref="ArgumentNullException">وقتی پارامتر مدل پوچ است پرتاب می‌شود.</exception>
        public async Task AddAsync(ProductDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده خالی می‌باشد");

            Product product = new Product();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Count = model.Count;
            product.CategoryId = model.CategoryId;
            product.CreatedDateTime = DateTime.Now;

            await _applicationDbContext.Products.AddAsync(product);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region ویرایش اطلاعات محصول با توجه به اطلاعات جدید ذریافت شده از مدل
        public async Task UpdateAsync(ProductDto model)
        {
            Product? oldProduct =
                await _applicationDbContext.Products.FindAsync(model.Id);

            if (oldProduct is null)
                throw new Exception("محصولی یافت نشد");

            oldProduct.Price = model.Price;
            oldProduct.Name = model.Name;
            oldProduct.Description = model.Description;
            oldProduct.ImageFileName = model.ImageFileName;
            oldProduct.Count = model.Count;
            oldProduct.CategoryId = model.CategoryId;

            _applicationDbContext.Products.Update(oldProduct);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region حذف محصول با توجه به آیدی
        public async Task DeleteAsync(int productId)
        {
            Product product =
                await _applicationDbContext.Products.FindAsync(productId);

            _applicationDbContext.Products.Remove(product);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region واکشی اطلاعات یک محصول با توجه به آیدی
        public Product? Get(int id)
        {
            Product? product =
                _applicationDbContext.Products.Find(id);

            return product;
        }
        #endregion

        #region واکشی اطلاعات یک محصول به صورت غیر هم زمان با توجه به آیدی 
        public async Task<Product?> GetAsync(int productId)
        {
            Product? result =
                await _applicationDbContext.Products.FindAsync(productId);

            return result;
        }
        #endregion

        #region واکشی کل جدول محصولات
        public async Task<List<Product>> GetsAsync()
        {
            List<Product> products =
                await _applicationDbContext.Products.ToListAsync();

            return products;
        }
        #endregion

        #region واکشی اطلاعات محصولات با توجه به آیدی یک دسته بندی خاص
        public async Task<List<Product>> GetsByCategoryAsync(int categoryId)
        {
            List<Product> products =
                await _applicationDbContext
                .Products
                .Where(product => product.CategoryId == categoryId).ToListAsync();

            return products;
        }
        #endregion
    }
}
