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

        #region ایجاد یک سبد خرید جدید با استفاده از اطلاعات دریافتی از مدل
        public async Task CreateAsync(CartDto model)
        {
            
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

  
            Cart cart = new Cart();

            /// <summary>
            /// اختصاص دادن مقادیر به ویژگی‌های سبد خرید از مدل دریافتی.
            /// </summary>
            cart.ProductId = model.ProductId;
            cart.UserId = model.UserId;
            cart.Count = model.Count;

            /// <summary>
            /// افزودن سبد خرید جدید به لیست سبد های موجود در DbContext.
            /// </summary>
            await _applicationDbContext.Carts.AddAsync(cart);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion


        #region ویرایش اطلاعات یک سبد خرید موجود با استفاده از اطلاعات دریافتی از مدل
        public async Task EditAsync(CartDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");


            /// <summary>
            /// جستجو برای یافتن سبد خرید موجود در پایگاه داده بر اساس شناسه دریافتی از مدل.
            /// </summary>
            Cart? cart =
                await _applicationDbContext.Carts.FindAsync(model.Id);


            if (cart is null)
                throw new Exception("سبد خریدی یافت نشد");

            /// <summary>
            /// به‌روزرسانی اطلاعات سبد خرید با استفاده از اطلاعات دریافتی از مدل.
            /// </summary>
            cart.Count = model.Count;
            cart.ProductId = model.ProductId;
            cart.UserId = model.UserId;

            /// <summary>
            /// اعمال تغییرات بر روی سبد خرید
            /// </summary>
            _applicationDbContext.Carts.Update(cart);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion


        #region سبد خرید را بر اساس شناسه آن حذف می‌کند
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
        #endregion


        #region واکشی اطلاعات یک سبد خرید بر اساس شناسه آن
        public Cart? Get(int cartId)
        {
            if (cartId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? result =
                 _applicationDbContext.Carts.Find(cartId);

            return result;
        }
        #endregion


        #region واکشی اطلاعات کلی سبد خرید با توجه به آیدی
        public async Task<Cart?> GetAsync(int cartId)
        {
            if (cartId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Cart? result =
                await _applicationDbContext.Carts.FindAsync(cartId);

            return result;
        }
        #endregion


        #region واکشی اطلاعات سبد خرید بر اساس آیدی کاربر
        public async Task<List<Cart>> GetsByUserIdAsync(int userId)
        {
            List<Cart> result = await _applicationDbContext.Carts
               .Where(x => x.UserId == userId)
               .ToListAsync();

            return result;
        }
        #endregion


        #region واکشی اطلاعات سبد خرید بر اساس ایدی محصول
        public async Task<List<Cart>> GetByProductIdAsync(int productId)
        {
            return
              await _applicationDbContext.Carts.Where(x => x.ProductId == productId).ToListAsync();
        }
        #endregion
    }
}
