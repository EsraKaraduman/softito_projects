using SimakYolSupurge.Helpers;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Username = "admin",
                        Email = "admin@simakyol.com",
                        PasswordHash = HashHelper.HashPassword("admin123"),
                        Role = "Admin",
                        CreatedDate = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "customer",
                        Email = "customer@simakyol.com",
                        PasswordHash = HashHelper.HashPassword("user123"),
                        Role = "User",
                        CreatedDate = DateTime.UtcNow
                    }
                );
                context.SaveChanges();
            }

            if (!context.Vehicles.Any())
            {
                context.Vehicles.AddRange(
                    new Vehicle
                    {
                        Brand = "Bucher",
                        Model = "CityCat 5006",
                        Type = "Yol Süpürme Aracı",
                        Year = 2022,
                        Price = 3500,
                        Status = "Kiralık",
                        ImageUrl = "/images/bucher_citycat.jpg",
                        Description = "Kentsel alanlarda yüksek performanslı vakumlu yol süpürme aracı. 6 m³ hazne kapasitesi."
                    },
                    new Vehicle
                    {
                        Brand = "Dulevo",
                        Model = "6000",
                        Type = "Yol Süpürme Aracı",
                        Year = 2023,
                        Price = 4200,
                        Status = "Kiralık",
                        ImageUrl = "/images/dulevo_6000.jpg",
                        Description = "Endüstriyel alanlar ve belediyeler için ağır hizmet süpürme aracı. Minimum su kullanımı."
                    },
                    new Vehicle
                    {
                        Brand = "Caterpillar",
                        Model = "966M",
                        Type = "İş Makinası (Yükleyici)",
                        Year = 2021,
                        Price = 6000,
                        Status = "Kiralık",
                        ImageUrl = "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?q=80&w=800",
                        Description = "Şantiye ve madencilik alanları için yüksek kapasiteli yükleyici iş makinası. Yakıt tasarruflu."
                    },
                    new Vehicle
                    {
                        Brand = "Schmidt",
                        Model = "Swingo 200+",
                        Type = "Kompakt Süpürme Aracı",
                        Year = 2024,
                        Price = 3500000,
                        Status = "Satılık",
                        ImageUrl = "/images/schmidt_swingo.jpg",
                        Description = "Dar sokaklar ve yaya yolları için çevre dostu, kompakt yol süpürme aracı. Ergonomik tasarım."
                    }
                );
                context.SaveChanges();
            }

            var existingBucher = context.Vehicles.FirstOrDefault(v => v.Brand == "Bucher" && v.ImageUrl.Contains("unsplash"));
            if (existingBucher != null) { existingBucher.ImageUrl = "/images/bucher_citycat.jpg"; }
            var existingDulevo = context.Vehicles.FirstOrDefault(v => v.Brand == "Dulevo" && v.ImageUrl.Contains("unsplash"));
            if (existingDulevo != null) { existingDulevo.ImageUrl = "/images/dulevo_6000.jpg"; }
            var existingSchmidt = context.Vehicles.FirstOrDefault(v => v.Brand == "Schmidt" && v.ImageUrl.Contains("unsplash"));
            if (existingSchmidt != null) { existingSchmidt.ImageUrl = "/images/schmidt_swingo.jpg"; }
            context.SaveChanges();

            if (!context.Maintenances.Any())
            {
                context.Maintenances.AddRange(
                    new Maintenance
                    {
                        VehicleModel = "Bucher CityCat 5006",
                        Description = "Yıllık periyodik bakım ve yan fırçaların değişimi yapıldı.",
                        StartDate = DateTime.UtcNow.AddDays(-5),
                        EndDate = DateTime.UtcNow.AddDays(-4),
                        Cost = 12500,
                        Status = "Tamamlandı",
                        TechnicianName = "Ahmet Usta (Simak Servis)"
                    },
                    new Maintenance
                    {
                        VehicleModel = "Dulevo 6000",
                        Description = "Vakum sistemi filtre değişimi ve hidrolik sızıntı onarımı yapılıyor.",
                        StartDate = DateTime.UtcNow.AddDays(-2),
                        EndDate = null,
                        Cost = 28000,
                        Status = "Devam Ediyor",
                        TechnicianName = "Mehmet Tekniker"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Requests.Any())
            {
                context.Requests.AddRange(
                    new Request
                    {
                        CustomerName = "Kaya İnşaat A.Ş.",
                        CustomerEmail = "info@kayainsaat.com",
                        CustomerPhone = "0532 111 2233",
                        VehicleModel = "Caterpillar 966M",
                        RequestType = "Kiralama",
                        RequestDate = DateTime.UtcNow.AddDays(-1),
                        Notes = "Yeni şantiye sahasında kullanılmak üzere 15 günlük yükleyici kiralama talebi.",
                        Status = "Beklemede"
                    },
                    new Request
                    {
                        CustomerName = "Eren Belediye Hizmetleri",
                        CustomerEmail = "hizmet@erenbelediye.com",
                        CustomerPhone = "0555 444 5566",
                        VehicleModel = "Schmidt Swingo 200+",
                        RequestType = "Satın Alma",
                        RequestDate = DateTime.UtcNow.AddDays(-3),
                        Notes = "Belediye temizlik işleri için adetli alım öncesi 1 adet Schmidt Swingo alım talebi.",
                        Status = "Onaylandı"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
