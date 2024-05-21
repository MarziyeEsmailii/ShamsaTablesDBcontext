using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShamsaStoreServer.Entities;

namespace ShamsaStoreServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string ADMIN_ID = "3348e0c0-a5f9-41bf-9b4b-cc25ac9e8ae2";

            const string ADMIN_ROLE_ID = "aa8cc01e-04a7-45e5-9d3f-ef53f503c638";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = ADMIN_ROLE_ID, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });

            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<AuthenctiationUser>().HasData(
                 new AuthenctiationUser
                 {
                     Id = ADMIN_ID,
                     UserName = "Erfan",
                     NormalizedUserName = "YOUR_PHONE",
                     Email = "YOUR_EMAIL",
                     NormalizedEmail = "YOUR_EMAIL",
                     EmailConfirmed = true,
                     PhoneNumberConfirmed = true,
                     PasswordHash = hasher.HashPassword(null, "Admin123456@"),
                     SecurityStamp = string.Empty,
                     FullName = "YOUR_FULL_NAME"
                 });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                });
        }
    }
}
