using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Cart;
using Shared.Dtos.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShamsaStoreServer.ViewModels.Product;

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

        #region ایجاد یک سبد خرید
        [HttpPost]
        /// <summary>
        /// ایجاد یک سبد خرید با استفاده از اطلاعات ارائه شده در مدل.
        /// </summary>
        /// <param name="viewModel">مدل شامل اطلاعات محصول و تعداد مورد نیاز.</param>
        /// <returns>یک پاسخ HTTP که نشان می‌دهد عملیات موفقیت آمیز بوده یا خیر.</returns>
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
        #endregion

        #region ویرایش تعداد کالا در سبد خرید
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
        #endregion

        #region واکشی اطلاعات سبد خرید بر اساس آیدی کاربر
        [HttpGet("{userId}")]
        public async Task<IActionResult> Gets(int userId)
        {
            List<Cart> carts =
                await _cartService.GetsByUserIdAsync(userId);

            return Ok(carts);
        }
        #endregion

        #region حذف یا لغو کردن سبد خرید
        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartService.DeleteAsync(id);

            return Ok();
        }
        #endregion

        #region جست و جو در سبد خرید
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
        #endregion

        [HttpGet("CartsWithOrdering")]
        public async Task<IActionResult> CartsWithOrdering([FromQuery] bool orderbyDescending, [FromQuery] int userId)
        {
            return Ok(await _cartService.GetCartsWithOrdering(orderbyDescending, userId));
        }

        // برای فهمیدن که چند عدد سبد خرید برای کاربر وجود داره
        [HttpGet("CartCount")]
        public async Task<IActionResult> CartCount([FromQuery] int userId)
        {
            return Ok(await _cartService.GetCartCount(userId));
        }
    }
}
