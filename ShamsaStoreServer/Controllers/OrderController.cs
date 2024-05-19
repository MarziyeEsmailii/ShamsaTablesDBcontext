using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto viewModel)
        {
            await _orderService.CreateAsync(viewModel);

            return Ok();
        }

        [HttpGet("GetsOrder")]
        public async Task<IActionResult> Gets()
        {
            return Ok(await _orderService.GetsAsync());
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> Get(int orderId)
        {
            return Ok(await _orderService.GetAsync(orderId));
        }

        [HttpGet("GetOrderByUserId")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            return Ok(await _orderService.GetByUserIdAsync(userId));
        }

        [HttpGet("GetOrderByProductId")]

        public async Task<IActionResult> GetByProductId(int productId)
        {
            return Ok(await _orderService.GetByProductIdAsync(productId));
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> Delete(int orderId)
        {
            await _orderService.DeleteAsync(orderId);

            return Ok();
        }
    }
}
