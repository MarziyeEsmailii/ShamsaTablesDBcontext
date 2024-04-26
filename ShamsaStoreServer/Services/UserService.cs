using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.User;
using System;
using System.Threading.Tasks;

namespace ShamsaStoreServer.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task CreateAsync(UserCreateViewModel viewModel)
        {
            if (viewModel is null)
                throw new Exception("موارد ارسال شده نادرست است");

            User user = new User();

            user.FullName = viewModel.FullName;

            user.Email = viewModel.Email;

            user.Password = viewModel.Password;

            await _applicationDbContext.Users.AddAsync(user);

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
