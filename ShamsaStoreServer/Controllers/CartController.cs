using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CartCreateViewModel viewModel)
        {
            await _cartService.CreateAsync(viewModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(CartEditViewModel viewModel)
        {
            await _cartService.EditAsync(viewModel);

            return Ok();
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> Gets(int userId)
        {
            List<Cart> carts =
                await _cartService.GetsByUserIdAsync(userId);

            return Ok(carts);
        }
    }
}
