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
        public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
        {
            await _orderService.CreateAsync(viewModel);

            return Ok();
        }
    }
}
