using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.ViewModels.User;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto viewModel)
        {
            await _userService.CreateAsync(viewModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserDto model)
        {
            await _userService.EditAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var result = await _userService.GetsAsync();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);

            return Ok();
        }
    }
}
