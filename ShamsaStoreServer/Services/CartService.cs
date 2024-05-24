using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Search;

namespace ShamsaStoreServer.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CartService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        
        public async Task CreateAsync(CartDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart cart = new Cart();

            var countOfProduct =
                await _applicationDbContext.Products
                .Where(current => current.Id == model.ProductId)
                .Select(current=>current.Count)
                .FirstOrDefaultAsync();

            if(countOfProduct< model.Count)
                throw new Exception("موجودی کم است");

            cart.ProductId = model.ProductId;

            cart.UserId = model.UserId;

            cart.Count = model.Count;

            await _applicationDbContext.Carts.AddAsync(cart);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task EditAsync(CartDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? cart =
                await _applicationDbContext.Carts.FindAsync(model.Id);

            if (cart is null)
                throw new Exception("سبد خریدی یافت نشد");

            cart.Count = model.Count;

            cart.ProductId = model.ProductId;

            cart.UserId = model.UserId;

            _applicationDbContext.Carts.Update(cart);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int cartId)
        {
            if (cartId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? cart =
                 await _applicationDbContext.Carts.FindAsync(cartId);

            if (cart is null)
                throw new Exception(" سبد خریدی یافت نشد");

            _applicationDbContext.Carts.Remove(cart);

            await _applicationDbContext.SaveChangesAsync();
        }

        public Cart? Get(int cartId)
        {
            if (cartId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? result =
                 _applicationDbContext.Carts.Find(cartId);

            return result;
        }

        public async Task<Cart?> GetAsync(int cartId)
        {
            if (cartId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? result =
                await _applicationDbContext.Carts.FindAsync(cartId);

            return result;
        }

        public async Task<List<Cart>> GetsByUserIdAsync(int userId)
        {
            List<Cart> result =
                await _applicationDbContext.Carts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<List<Cart>> GetByProductIdAsync(int productId)
        {
            return
              await _applicationDbContext.Carts.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task RemoveByProductId(int productId)
        {
            var cart =
                await _applicationDbContext.Carts
                .Where(current => current.ProductId == productId)
                .FirstOrDefaultAsync();

            _applicationDbContext.Remove(cart);
        }


        public async Task<List<CartDto>> GetsWithSearchAsync(SearchDto model)
        {
            return await _applicationDbContext.Carts
                .Include(current => current.Product)
                .Where(current => current.Product.Name.Contains(model.Search))
                .Where(current=>current.UserId == model.UserId)
                .Select(current => new CartDto
                {
                    Count = current.Count,
                    Id = current.Id,
                    ProductId = current.ProductId,
                    ProductName = current.Product.Name,
                    UserId = current.UserId,
                })
                .ToListAsync();
        }
    }
}
