using Microsoft.EntityFrameworkCore;
using SneakersStore.Models.Entities;

namespace SneakersStore
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) 
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
