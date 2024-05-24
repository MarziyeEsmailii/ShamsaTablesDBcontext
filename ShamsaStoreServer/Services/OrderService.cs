using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Order;
using Shared.Dtos.Order;
using Shared.Dtos.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly CartService _cartService;

        private readonly ProductService _productService;

        public OrderService(ApplicationDbContext applicationDbContext, CartService cartService, ProductService productService)
        {
            _applicationDbContext = applicationDbContext;

            _cartService = cartService;

            _productService = productService;
        }


        #region ایجاد سفارش جدید بر اساس مدل دریافتی
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

        #region ویرایش  جدول سفارشات بر اساس مدل دریافتی
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

        #region حذف یا همان لغو سفارش بر اساس آیدی
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

        #region واکشی اطلاعات یک سفارش خاص بر اساس آیدی
        public Order? Get(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                 _applicationDbContext.Orders.Find(orderId);

            return result;
        }
        #endregion

        #region واکشی یک سفارش خاص به صورت غیر هم زمان
        public async Task<Order?> GetAsync(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                await _applicationDbContext.Orders.FindAsync(orderId);

            return result;
        }
        #endregion

        #region واکشی لیست سفارشات 
        public async Task<List<Order>> GetsAsync()
        {
            List<Order> result =
                await _applicationDbContext.Orders.ToListAsync();

            return result;
        }
        #endregion

        #region واکشی سفارشات ثبت شده بر اساس آیدی محصول
        public async Task<Order?> GetByProductIdAsync(int productId)
        {
            return
                await _applicationDbContext.Orders
                .Where(x => x.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region واکشی اطلاعات سفارش کاربران
        public async Task<Order?> GetByUserIdAsync(int userId)
        {
            return
                await _applicationDbContext.Orders
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region ثبت سفارش جدید و بروز رسانی تعداد موجودی ها
        public async Task<bool> AddRangeAsync(List<OrderDto> models)
        {
            var orders =
                models.Select(current => new Order
                {
                    ProductId = current.ProductId,
                    Count = current.Count,
                    Price = current.Price * current.Count,
                    DateTimeCreated = DateTime.Now,
                    UserId = current.UserId,
                })
                .ToList();

            foreach (var item in orders)
            {
                var result = await _applicationDbContext.Products
                    .Where(current => current.Count >= item.Count)
                    .Select(current=>current.Count)
                    .FirstOrDefaultAsync();

                if (result < item.Count)
                {
                    return false;
                }
            }

            await _applicationDbContext.AddRangeAsync(orders);

            foreach (var item in orders)
            {
                await _productService.SubtractCountAsync(item.ProductId, item.Count);

                await _cartService.RemoveByProductId(item.ProductId);
            }

            //چک کردن موفقیت آمیز بودن عملیات
            var affectedRow =
                 await _applicationDbContext.SaveChangesAsync();

            return affectedRow > 0 ? true : false;
        }
        #endregion


        #region جست و جو در سفارشات بر اساس آیدی کاربر و نام محصول
        public async Task<List<OrderDto>> GetsWithSearchAsync(SearchDto model)
        {
            return await _applicationDbContext.Orders
                .Include(current => current.Product)
                .Where(current => current.Product.Name.Contains(model.Search))
                .Where(current=>current.UserId == model.UserId)
                .Select(current => new OrderDto
                {
                    Id = current.Id,
                    Count = current.Count,
                    ProductName = current.Product.Name,
                    Price = current.Price,
                    ProductId = current.ProductId,
                    UserId = current.UserId,
                })
                .ToListAsync();
        }
        #endregion


        #region گزارشی از سفارش ها که براساس محصول گروه بندی وبراساس محدوده تاریخ فیلتر شده اند
        public async Task<List<OrderReportResponseDto>> OrdersReportByProductAsync(OrderReportRequestDto model)
        {
            //لیست برای نگه داری داده های گزارش نهایی
            var orderReports =
                new List<OrderReportResponseDto>();

            var ordersByGroupBy =
                  _applicationDbContext.Orders
                 .Where(current => current.DateTimeCreated >= model.FromDate)
                 .Where(current => current.DateTimeCreated <= model.ToDate)
                 .GroupBy(current => current.ProductId)
                 .Select(current => new
                 {
                     ProductId = current.Key,
                     TotalSum = current.Sum(current => current.Price),
                 })
                 .ToList();

            //واکشی اطلاعات محصولات
            foreach (var order in ordersByGroupBy)
            {
                var orderReport =
                    await _applicationDbContext.Products
                    .Where(current => current.Id == order.ProductId)
                    .Select(current => new OrderReportResponseDto
                    {
                        TotalSum = order.TotalSum,
                        ProductCategoryName = current.Category.Name,
                        ProductId = current.Id,
                        ProductName = current.Name,
                    })
                    .FirstOrDefaultAsync();

                if (orderReport is not null)
                    orderReports.Add(orderReport);
            }

            return orderReports
                .Skip((model.PageNo - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();
        }
        #endregion
    }
}
