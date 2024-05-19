using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.Category;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = 
                await _categoryService.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var result = 
                await _categoryService.GetsAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]CategoryDto viewModel)
        {
            await _categoryService.AddAsync(viewModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] CategoryDto viewModel)
        {
            await _categoryService.EditAsync(viewModel);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok();
        }
    }
}
