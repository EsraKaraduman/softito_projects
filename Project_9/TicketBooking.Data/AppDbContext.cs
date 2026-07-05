using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Entities;

namespace TicketBooking.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Events)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeatNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(t => t.User)
                  .WithMany(u => u.Tickets)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Event)
                  .WithMany(e => e.Tickets)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Konser", Description = "Canlı müzik, konserler ve festivaller", IsActive = true },
            new Category { Id = 2, Name = "Tiyatro", Description = "Tiyatro oyunları ve sahne sanatları", IsActive = true },
            new Category { Id = 3, Name = "Spor", Description = "Futbol, basketbol ve diğer spor müsabakaları", IsActive = true },
            new Category { Id = 4, Name = "Sinema", Description = "Vizyondaki filmler ve özel gösterimler", IsActive = true }
        );

        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                Title = "Tarkan Harbiye Konseri",
                Description = "Tarkan, en sevilen şarkılarıyla Harbiye Açıkhava Sahnesi'nde hayranlarıyla buluşuyor.",
                Date = DateTime.Today.AddDays(30),
                Location = "Harbiye Cemil Topuzlu Açıkhava Tiyatrosu, İstanbul",
                Price = 1500.00m,
                Capacity = 500,
                AvailableSeats = 500,
                ImageUrl = "/images/tarkan.jpg",
                CategoryId = 1
            },
            new Event
            {
                Id = 2,
                Title = "Cem Yılmaz - CMXXIV",
                Description = "Cem Yılmaz, yeni stand-up gösterisi ile Zorlu PSM'de kahkaha dolu bir gece sunuyor.",
                Date = DateTime.Today.AddDays(15),
                Location = "Zorlu PSM - Turkcell Sahnesi, İstanbul",
                Price = 1200.00m,
                Capacity = 300,
                AvailableSeats = 300,
                ImageUrl = "/images/cemyilmaz.jpg",
                CategoryId = 2
            },
            new Event
            {
                Id = 3,
                Title = "Galatasaray - Fenerbahçe Derbisi",
                Description = "Trendyol Süper Lig dev derbi heyecanı RAMS Park'ta yaşanıyor.",
                Date = DateTime.Today.AddDays(21),
                Location = "RAMS Park, İstanbul",
                Price = 2500.00m,
                Capacity = 1000,
                AvailableSeats = 1000,
                ImageUrl = "/images/derbi.jpg",
                CategoryId = 3
            },
            new Event
            {
                Id = 4,
                Title = "Interstellar Özel Gösterim",
                Description = "Christopher Nolan'ın efsanevi yapıtı Interstellar, dev perdede sinemaseverlerle buluşuyor.",
                Date = DateTime.Today.AddDays(5),
                Location = "Kadıköy Sineması, İstanbul",
                Price = 200.00m,
                Capacity = 100,
                AvailableSeats = 100,
                ImageUrl = "/images/interstellar.jpg",
                CategoryId = 4
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@biletal.com",
                PasswordHash = HashPassword("admin123"),
                Role = "Admin",
                CreatedDate = DateTime.Today
            },
            new User
            {
                Id = 2,
                Username = "esra",
                Email = "esra@gmail.com",
                PasswordHash = HashPassword("123456"),
                Role = "User",
                CreatedDate = DateTime.Today
            }
        );
    }
}
