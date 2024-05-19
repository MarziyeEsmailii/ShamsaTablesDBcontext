using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

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

        public async Task DeleteAsync(long cartId)
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

    }
}
