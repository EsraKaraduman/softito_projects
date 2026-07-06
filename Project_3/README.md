# Project 3: Dondurma Dükkanı Stok & Satış Sistemi (DondurmacfUI)

Bu proje, katmanlı mimari (N-Tier Architecture) prensipleri kullanılarak geliştirilmiş bir **Dondurma Dükkanı Envanter ve Raporlama Sistemi**dir. Proje; Model, Data ve UI katmanlarına ayrılarak temiz kod standartlarına uygun şekilde yapılandırılmıştır.

## 🧱 Katmanlı Mimari (Project Structure)
1. **Dondurmacf.Model:** Veritabanındaki tabloları temsil eden Entity sınıfları (Dondurma, DondurmaTürü vb.).
2. **Dondurmacf.Data:** Entity Framework DbContext sınıfını ve veritabanı CRUD operasyonlarını içeren veri erişim katmanı.
3. **DondurmacfUI:** Kullanıcı etkileşiminin gerçekleştiği, denetleyicileri ve sayfaları barındıran ASP.NET Core MVC katmanı.

## 💻 Teknolojiler
* **Framework:** ASP.NET Core MVC (v8.0)
* **Mimari:** N-Tier (Katmanlı) Mimari
* **Veritabanı:** MS SQL Server & Entity Framework Core
* **Oturum Yönetimi:** Session & Cookie tabanlı kimlik doğrulama
* **Tasarım:** HTML5, CSS3, Bootstrap, Javascript

## 🚀 Özellikler
* **Dondurma Türleri (TurController):** Külah, kap, paket dondurma, meyveli, çikolatalı gibi türlerin tanımlanması ve listelenmesi.
* **Ürün Yönetimi (UrunController):** Dondurma çeşitlerinin, fiyatlarının ve porsiyon bilgilerinin yönetimi.
* **Kritik Raporlama (RaporController):** Dondurma stokları, günlük/haftalık satış grafikleri ve en çok tercih edilen dondurma türlerinin detaylı raporlanması.
* **Kullanıcı İşlemleri (AccountController):** Personel ve yönetici hesaplarının giriş/çıkış süreçleri.

## 📸 Ekran Görüntüleri

### Satış ve Ürün Listesi
<p align="center">
  <img src="../assets/DondurmacfUI_image_1.png" width="48%" alt="Dondurma Seçimi" />
  <img src="../assets/DondurmacfUI_image_2.png" width="48%" alt="Kategoriler" />
</p>

<details>
  <summary>🔍 Diğer Ekran Görüntülerini Göster</summary>
  <br>
  <p align="center">
    <img src="../assets/DondurmacfUI_image_3.png" width="48%" alt="Yeni Kayıt" />
    <img src="../assets/DondurmacfUI_image_4.png" width="48%" alt="Kullanıcı Paneli" />
  </p>
  <p align="center">
    <img src="../assets/DondurmacfUI_image_5.png" width="48%" alt="Stok Grafikleri" />
    <img src="../assets/DondurmacfUI_image_6.png" width="48%" alt="Satış Geçmişi" />
  </p>
  <p align="center">
    <img src="../assets/DondurmacfUI_image_7.png" width="48%" alt="Giriş Ekranı" />
    <img src="../assets/DondurmacfUI_image_8.png" width="48%" alt="Rapor Analizi" />
  </p>
</details>

## 🛠️ Kurulum ve Çalıştırma
1. **Veritabanı Ayarı:** `appsettings.json` dosyasındaki bağlantı dizesini kendi SQL Server ayarlarınızla güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=CIHAZ_ADI;Database=DondurmaDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
2. **Migration:** Data katmanını seçerek Package Manager Console'da veritabanını güncelleyin:
   ```bash
   Update-Database
   ```
3. **Projeyi Başlatma:** `DondurmacfUI` projesini başlangıç projesi (Startup Project) olarak ayarlayıp çalıştırın.
