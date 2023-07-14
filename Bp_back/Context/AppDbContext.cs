using Bp_back.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bp_back.Context
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public AppDbContext()
        : base(CreateOptions())
        {
            //Database.EnsureCreated();
        }

        static DbContextOptions CreateOptions()
        {
            var connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            var b = new DbContextOptionsBuilder().UseSqlServer(connection);
            return b.Options;
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = 1,
                        Login = "admin",
                        Active = true,
                        Role = "admin",
                        Email = "admin@admin.admin",
                        Password = "38ef/vWYE15AgcPSeUkYA3rXsVgnnGEtj0xWxq846bI=",
                        PasswordSalt = "7KmMVPksY91X8kfcxsceURd71psvDgJ24vsRw5aKrVA="
                    }

            );
        }
    }
}
