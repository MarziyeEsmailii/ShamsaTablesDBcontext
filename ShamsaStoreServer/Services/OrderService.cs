using Microsoft.EntityFrameworkCore;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.Order;
using System;
using System.Collections.Generic;
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

        public async Task CreateAsync(OrderCreateViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Order order = new Order();

            order.ProductId = viewModel.ProductId;

            order.UserId = viewModel.UserId;

            order.Price = viewModel.Price;

            await _applicationDbContext.Orders.AddAsync(order);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task EditAsync(OrderEditViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? order =
                await _applicationDbContext.Orders.FindAsync(viewModel.Id);

            if (order is null)
                throw new Exception("سفارشی یافت نشد");

            order.Price = viewModel.Price;

            order.ProductId = viewModel.ProductId;

            order.UserId = viewModel.UserId;

            _applicationDbContext.Orders.Update(order);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long orderId)
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

        public Order? Get(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                 _applicationDbContext.Orders.Find(orderId);

            return result;
        }

        public async Task<Order?> GetAsync(int orderId)
        {
            if (orderId < 0)
                throw new Exception("موارد ارسال شده نادرست است");

            Order? result =
                await _applicationDbContext.Orders.FindAsync(orderId);

            return result;
        }

        public async Task<List<Order>> GetsAsync()
        {
            List<Order> result =
                await _applicationDbContext.Orders.ToListAsync();

            return result;
        }
    }
}
