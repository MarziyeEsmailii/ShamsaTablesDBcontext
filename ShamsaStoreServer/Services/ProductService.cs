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

        public async Task AddAsync(ProductCreateViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده خالی میباشد");

            Product product = new Product();

            product.Name = viewModel.Name;

            product.Description = viewModel.Description;

            product.Price = viewModel.Price;

            product.Count = viewModel.Count;

            product.CategoryId = viewModel.CategoryId;

            product.CreatedDateTime = DateTime.Now;

            await _applicationDbContext.Products.AddAsync(product);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductEditViewModel viewModel)
        {
            Product? oldProduct =
                await _applicationDbContext.Products.FindAsync(viewModel.Id);

            if (oldProduct is null)
                throw new Exception("محصولی یافت نشد");

            oldProduct.Price = viewModel.Price;

            oldProduct.Name = viewModel.Name;

            oldProduct.Description = viewModel.Description;

            oldProduct.ImageFileName = viewModel.ImageFileName;

            oldProduct.Count = viewModel.Count;

            oldProduct.CategoryId = viewModel.CategoryId;

            _applicationDbContext.Products.Update(oldProduct);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            Product product = 
                await _applicationDbContext.Products.FindAsync(productId);

            _applicationDbContext.Products.Remove(product);

            await _applicationDbContext.SaveChangesAsync();
        }

        public Product? Get(int id)
        {
            Product? product =
                _applicationDbContext.Products.Find(id);
            
            return product;
        }

        public async Task<Product?> GetAsync(int productId)
        {
            Product? result =
                await _applicationDbContext.Products.FindAsync(productId);

            return result;
        }

        public async Task<List<Product>> GetsAsync()
        {
            List<Product> products =
                await _applicationDbContext.Products.ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetsByCategoryAsync(int categoryId)
        {
            List<Product> products = 
                await _applicationDbContext
                .Products
                .Where(product => product.CategoryId == categoryId).ToListAsync();
            
            return products;
        }
    }
}
