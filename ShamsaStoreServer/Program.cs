using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShamsaStoreServer.Data;
using ShamsaStoreServer.Services;
using ShamsaStoreServer.Entities;
using Microsoft.AspNetCore.Identity;

namespace ShamsaStoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ایجاد یک نمونه از WebApplication Builder با استفاده از متد CreateBuilder
            var builder = WebApplication.CreateBuilder(args);

            // اضافه کردن سرویس‌های کنترلر، توضیحات API و Swagger به پروژه
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // اضافه کردن DbContext برای کار با پایگاه داده SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>
                (options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));

            // پیکربندی Identity API Endpoints با تنظیمات خاص برای رمز عبور
            builder.Services.AddIdentityApiEndpoints<AuthenctiationUser>(options =>
            {
                // تنظیمات مربوط به رمز عبور
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;

            })
               .AddRoles<IdentityRole>() // اضافه کردن نقش‌ها
               .AddEntityFrameworkStores<ApplicationDbContext>(); // ذخیره‌سازی اطلاعات در Entity Framework

            // اضافه کردن سرویس‌های مورد نیاز به DI Container
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<CartService>();

            // اضافه کردن سیاست CORS برای اجازه دسترسی از هر منبع
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Cors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin() // اجازه دسترسی از هر منبع
                            .AllowAnyHeader()// اجازه ارسال هر نوع هدر
                            .AllowAnyMethod(); // اجازه استفاده از هر روش HTTP
                    });
            });

            // ساخت نمونه WebApplication با استفاده از Builder ساخته شده
            var app = builder.Build();

            // اگر محیط توسعه است، فعال کردن Swagger و UI آن
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // اعمال سیاست CORS تعریف شده
            app.UseCors("Cors");
            // اعمال ردگیری HTTPS
            app.UseHttpsRedirection();
            // فعال سازی احراز هویت
            app.UseAuthorization();
            // نقشه‌برداری از کنترلرها
            app.MapControllers();
            // اجرای برنامه
            app.Run();
        }
    }
}
