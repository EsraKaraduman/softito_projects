using Microsoft.EntityFrameworkCore;
using MarketMvcProject.Models;

namespace MarketMvcProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }

    }
}
