using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Kullanicilar> Kullanicilar { get; set; }
        public DbSet<Kategoriler> Kategoriler { get; set; }
        public DbSet<Urunler> Urunler { get; set; }
        public DbSet<Satislar> Satislar { get; set; }
    }
}
