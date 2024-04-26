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
        public async Task<IActionResult> Create(UserCreateViewModel viewModel)
        {
            await _userService.CreateAsync(viewModel);

            return Ok();
        }
    }
}
