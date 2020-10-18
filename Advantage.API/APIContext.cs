using Advantage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Advantage.API
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Server> Servers { get; set; }
    }
}
