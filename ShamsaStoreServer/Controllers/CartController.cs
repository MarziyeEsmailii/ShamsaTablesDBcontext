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

        private readonly ProductService _productService;
        /// <summary>
        /// متد سازنده کلاس سبد خرید
        /// </summary>
        /// <param name="cartService"></param>
        /// <param name="productService"></param>
        public CartController(CartService cartService,ProductService productService)
        {
            _cartService = cartService;

            _productService = productService;
        }

        [HttpPost]
        /// <summary>
        /// ایجاد یک سبد خرید جدید با توجه به جزئیات ارائه شده در DTO.
        /// </summary>
        /// <param name="viewModel">DTO شامل اطلاعات محصول و تعداد مورد نیاز.</param>
        /// <returns>IActionResult: نتیجه عملیات ایجاد سبد خرید.</returns>
        public async Task<IActionResult> Create([FromBody] CartDto viewModel)
        {
            var product = 
                await _productService.GetAsync(viewModel.ProductId);

            if (product is null)
                return NotFound();

            if (product.Count < viewModel.Count)
                return BadRequest("تعداد کالای های شما بیشتر از حد مجاز است");

            await _cartService.CreateAsync(viewModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] CartDto viewModel)
        {
            var product =
                await _productService.GetAsync(viewModel.ProductId);

            if (product is null)
                return NotFound();

            if (product.Count < viewModel.Count)
                return BadRequest("تعداد کالای های شما بیشتر از حد مجاز است");

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

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartService.DeleteAsync(id);

            return Ok();
        }
    }
}
