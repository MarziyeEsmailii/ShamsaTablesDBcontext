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
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            /// <summary>
            /// Add DbContext and Config Idnetity
            /// </summary>
            builder.Services.AddDbContext<ApplicationDbContext>
                (options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));

            

            builder.Services.AddIdentityApiEndpoints<AuthenctiationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequireLowercase = false;

                options.Password.RequireUppercase = false;

                options.Password.RequireDigit = false;

                options.Password.RequiredLength = 6;

            })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<ProductService>();

            builder.Services.AddScoped<CategoryService>();

            builder.Services.AddScoped<OrderService>();

            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<CartService>();

            builder.Services.AddScoped<CartService>();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Cors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("Cors");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
