using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Product;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result =
                await _productService.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var result =
                await _productService.GetsAsync();

            return Ok(result);
        }

        [HttpGet("GetsByCategory")]
        public async Task<IActionResult> GetsByCategory(int categoryId)
        {
            var result =
                await _productService.GetsByCategoryAsync(categoryId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductDto viewModel)
        {
            await _productService.AddAsync(viewModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ProductDto viewModel)
        {
            await _productService.UpdateAsync(viewModel);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return Ok();
        }
    }
}
