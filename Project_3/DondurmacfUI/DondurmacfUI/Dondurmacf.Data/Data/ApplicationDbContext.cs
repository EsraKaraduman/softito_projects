using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Dondurmacf.Model;

namespace Dondurmacf.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DondurmaTur> Turs { get; set; }
        public DbSet<Urun> Uruns { get; set; }
        public DbSet<Kullanici> Kullanicis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kullanici>().HasData(
                new Kullanici { KullaniciNo = 1, KullaniciAdi = "admin", Sifre = "123", AdSoyad = "Esra Admin" },
                new Kullanici { KullaniciNo = 2, KullaniciAdi = "user", Sifre = "123", AdSoyad = "Canan Personel" },
                new Kullanici { KullaniciNo = 3, KullaniciAdi = "supervisor", Sifre = "123", AdSoyad = "Ömer Denetleyici" },
                new Kullanici { KullaniciNo = 4, KullaniciAdi = "cashier", Sifre = "123", AdSoyad = "Aylin Kasiyer" }
            );

            modelBuilder.Entity<Urun>().HasData(
                new Urun { UrunNo = 1, UrunAdi = "Klasik Dondurmalar", Aciklama = "Geleneksel lezzetler" },
                new Urun { UrunNo = 2, UrunAdi = "Meyveli Dondurmalar", Aciklama = "Taze meyvelerden üretilen dondurmalar" },
                new Urun { UrunNo = 3, UrunAdi = "Premium Dondurmalar", Aciklama = "Özel tarifler ve zengin içerikler" },
                new Urun { UrunNo = 4, UrunAdi = "Diyet & Vegan", Aciklama = "Şekersiz ve bitkisel sütlü seçenekler" },
                new Urun { UrunNo = 5, UrunAdi = "Tatlılar & Kup", Aciklama = "Dondurma eşliğinde sunulan lezzetler" }
            );

            modelBuilder.Entity<DondurmaTur>().HasData(
                new DondurmaTur { TurNo = 1, Tur = "Sade Maraş", Fiyat = 50, UrunNo = 1, EkleyenKullaniciNo = 1 },
                new DondurmaTur { TurNo = 2, Tur = "Çikolatalı", Fiyat = 55, UrunNo = 1, EkleyenKullaniciNo = 1 },
                new DondurmaTur { TurNo = 3, Tur = "Çilekli", Fiyat = 60, UrunNo = 2, EkleyenKullaniciNo = 1 },
                new DondurmaTur { TurNo = 4, Tur = "Limonlu", Fiyat = 60, UrunNo = 2, EkleyenKullaniciNo = 2 },
                new DondurmaTur { TurNo = 5, Tur = "Antep Fıstıklı", Fiyat = 75, UrunNo = 3, EkleyenKullaniciNo = 2 },
                new DondurmaTur { TurNo = 6, Tur = "Karamelli Bisküvili", Fiyat = 80, UrunNo = 3, EkleyenKullaniciNo = 2 },
                new DondurmaTur { TurNo = 7, Tur = "Vanilyalı Kurabiyeli", Fiyat = 70, UrunNo = 1, EkleyenKullaniciNo = 3 },
                new DondurmaTur { TurNo = 8, Tur = "Kavunlu", Fiyat = 65, UrunNo = 2, EkleyenKullaniciNo = 3 },
                new DondurmaTur { TurNo = 9, Tur = "Böğürtlenli", Fiyat = 65, UrunNo = 2, EkleyenKullaniciNo = 4 },
                new DondurmaTur { TurNo = 10, Tur = "Belçika Çikolatalı", Fiyat = 85, UrunNo = 3, EkleyenKullaniciNo = 1 },
                new DondurmaTur { TurNo = 11, Tur = "Şekersiz Vanilyalı", Fiyat = 75, UrunNo = 4, EkleyenKullaniciNo = 4 },
                new DondurmaTur { TurNo = 12, Tur = "Vegan Hindistan Cevizli", Fiyat = 80, UrunNo = 4, EkleyenKullaniciNo = 3 },
                new DondurmaTur { TurNo = 13, Tur = "Dondurmalı Helva", Fiyat = 95, UrunNo = 5, EkleyenKullaniciNo = 2 },
                new DondurmaTur { TurNo = 14, Tur = "Dondurmalı Sufle", Fiyat = 110, UrunNo = 5, EkleyenKullaniciNo = 1 }
            );
        }
    }
}
