using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}
