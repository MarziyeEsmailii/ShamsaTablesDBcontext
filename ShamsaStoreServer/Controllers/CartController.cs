using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Cart;
using Shared.Dtos.Search;
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

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;

            _productService = productService;
        }

        [HttpPost]
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

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SearchDto model)
        {
            return
                model.Search
                !=
                null ?
                Ok(await _cartService.GetsWithSearchAsync(model))
                :
                BadRequest("نام کالا خود را وارد کنید");
        }
    }
}
