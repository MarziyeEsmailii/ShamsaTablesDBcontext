using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        #region سفارش جدید را بر اساس مدل دریافتی ایجاد می‌کند
        public async Task CreateAsync(OrderDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Order order = new Order();

            order.ProductId = model.ProductId;
            order.UserId = model.UserId;
            order.Price = model.Price;
            order.Count = model.Count;

            await _applicationDbContext.Orders.AddAsync(order);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region ویرایش جدول بر اساس اطلاعات دریافتی از مدل
        public async Task EditAsync(OrderDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? order =
                await _applicationDbContext.Orders.FindAsync(model.Id);

            if (order is null)
                throw new Exception("سفارشی یافت نشد");

            order.Price = model.Price;
            order.ProductId = model.ProductId;
            order.UserId = model.UserId;
            order.Count = model.Count;

            _applicationDbContext.Orders.Update(order);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region حذف رکورد بر اساس آیدی
        public async Task DeleteAsync(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? order =
                 await _applicationDbContext.Orders.FindAsync(orderId);

            if (order is null)
                throw new Exception(" سفارشی یافت نشد");

            _applicationDbContext.Orders.Remove(order);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region  واکشی اطلاعات سافرش بر اساس آیدی
        public Order? Get(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                 _applicationDbContext.Orders.Find(orderId);

            return result;
        }
        #endregion

        #region واکشی اطلاعات کل جدول بر اساس آیدی
        public async Task<Order?> GetAsync(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                await _applicationDbContext.Orders.FindAsync(orderId);

            return result;
        }
        #endregion

        #region واکشی کل جدول سفارشات
        public async Task<List<Order>> GetsAsync()
        {
            List<Order> result =
                await _applicationDbContext.Orders.ToListAsync();

            return result;
        }
        #endregion

        #region واکشی اطلاعات جدول سفارشات بر اساس آیدی محصول
        public async Task<Order?> GetByProductIdAsync(int productId)
        {
            return
                await _applicationDbContext.Orders
                .Where(x => x.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region واکشی اطلاعات جدول سفارشات بر اساس آیدی کاربر
        public async Task<Order?> GetByUserIdAsync(int userId)
        {
            return
                await _applicationDbContext.Orders
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
        #endregion

    }
}
