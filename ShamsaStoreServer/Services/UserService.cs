using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Entities;
using ShamsaStoreServer.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region ایجاد جدول کاربران با استفاده از مدل دریافتی
        public async Task CreateAsync(UserDto model)
        {
            if (model is null)
                throw new Exception("موارد ارسال شده نادرست است");

            User user = new User();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Password = model.Password;

            await _applicationDbContext.Users.AddAsync(user);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region واکشی اطلاعات با توجه به ایدی 
        public async Task<User?> GetAsync(int id)
        {
            return 
                await _applicationDbContext.Users.FindAsync(id);
        }
        #endregion


        #region واکشی اطلاعات کلی جدول کاربران
        public async Task<List<User>> GetsAsync()
        {
            return
                await _applicationDbContext.Users
                .ToListAsync();
        }
        #endregion

        #region ویرایش اطلاعات یک کاربر موجود با استفاده از اطلاعات دریافتی از مدل
        public async Task EditAsync(UserDto model)
        {
            User? oldUser =
                await _applicationDbContext.Users.FindAsync(model.Id);

            if (oldUser == null)
                throw new Exception("این کاربر پیدا نشد");

            oldUser.FullName = model.FullName;
            oldUser.Email = model.Email;

            _applicationDbContext.Users.Update(oldUser);

            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region حذف کابر با توجه به آیدی دریافت شده
        public async Task DeleteAsync(int id)
        {
            var user =
                await _applicationDbContext.Users.FindAsync(id);

            _applicationDbContext.Users.Remove(user);

           await _applicationDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
