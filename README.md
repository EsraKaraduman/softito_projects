# 🚀 Softito C# / .NET Web Projeleri Portföyü

Bu depo, **Softito Akademi** bünyesinde C# ve .NET teknolojileriyle geliştirilmiş **9 farklı web ve otomasyon projesini** bir araya getiren kapsamlı bir portföydür. Projeler, basit MVC uygulamalarından katmanlı mimarilere (N-Tier) ve RESTful API entegrasyonlarına kadar geniş bir yelpazeyi kapsamaktadır.

---

## 📂 Proje Kataloğu

Aşağıdaki tabloda depodaki tüm projelerin özet bilgileri, kullanılan teknolojiler ve detaylı açıklamaları yer almaktadır.

| Proje | Açıklama | Teknoloji Yığını | Klasör Linki |
| :--- | :--- | :--- | :--- |
| **Project 1: MarketMvcProject** | Ürün, kategori, müşteri ve sipariş yönetimini barındıran market otomasyonu. | `ASP.NET Core MVC 8.0` `EF Core` `SQL Server` `Session` | [Klasöre Git](./Project_1) |
| **Project 2: CikolataciMVC** | Çikolata dükkanı için ürün sergileme, üye takibi ve sipariş yönetim portalı. | `ASP.NET Core MVC 8.0` `EF Core` `SQL Server` `Identity` | [Klasöre Git](./Project_2) |
| **Project 3: DondurmacfUI** | Ürün türleri ve satış raporları içeren katmanlı mimarili dondurma dükkanı paneli. | `ASP.NET Core MVC` `N-Tier Architecture` `SQL Server` | [Klasöre Git](./Project_3) |
| **Project 4: Dukkanrzrpg** | Razor Pages mimarisiyle geliştirilmiş dinamik oyuncak dükkanı portalı. | `ASP.NET Core Razor Pages` `EF Core` `SQL Server` | [Klasöre Git](./Project_4) |
| **Project 5: StockManager** | Depo doluluk oranları ve stok hareketlerini izleyen envanter takip paneli. | `ASP.NET Core MVC` `EF Core` `SQL Server` `Chart.js` | [Klasöre Git](./Project_5) |
| **Project 6: SimakYolSupurge** | SQLite veritabanlı, rol tabanlı yol süpürme araç bakım ve servis takip sistemi. | `ASP.NET Core MVC` `SQLite` `Role Auth` `Web API` | [Klasöre Git](./Project_6) |
| **Project 7: CourseEnrollment** | Öğrenci kayıtları, dersler ve eğitmenleri yöneten kurs kayıt sistemi. | `ASP.NET Core MVC` `EF Core` `SQL Server` `Excel Export` | [Klasöre Git](./Project_7) |
| **Project 8: HotelProject** | JWT yetkilendirmeli Web API ve MVC arayüzlü otel rezervasyon sistemi. | `Web API` `ASP.NET Core MVC` `JWT Auth` `SQL Server` | [Klasöre Git](./Project_8) |
| **Project 9: TicketBooking** | Temiz Mimari (Clean Architecture) ile yazılmış etkinlik biletleme portalı. | `Clean Architecture` `ASP.NET Core MVC` `EF Core` | [Klasöre Git](./Project_9) |

---

## 📸 Ekran Görüntüleri & Görseller

GitHub sayfanızın daha canlı görünmesi için her proje için ekran görüntülerinizi ekleyebilirsiniz. Ekran görüntülerinizi eklemek için şu adımları izleyebilirsiniz:

1. Deponuzun kök dizininde `assets` adında bir klasör oluşturun.
2. Ekran görüntülerinizi aşağıdaki isimlerle bu klasörün içine yükleyin:
   * Market Projesi: `market_project_mockup.jpg`
   * Çikolata Projesi: `chocolate_shop_mockup.jpg`
   * Dondurma Projesi: `ice_cream_shop_mockup.jpg`
   * Oyuncak Projesi: `toy_shop_mockup.jpg`
   * Stok Yöneticisi: `stock_manager_mockup.jpg`
   * Yol Süpürge Projesi: `road_sweeper_mockup.jpg`
   * Kurs Kayıt Projesi: `course_enrollment_mockup.jpg`
   * Otel Projesi: `hotel_booking_mockup.jpg`
   * Bilet Projesi: `ticket_booking_mockup.jpg`

Yüklediğiniz bu görseller, hem ana README sayfasında hem de ilgili projelerin kendi içerisindeki README dosyalarında otomatik olarak görüntülenecektir.

---

## 🛠️ Genel Kurulum ve Çalıştırma Kılavuzu

Depodaki projelerin çoğu benzer şekilde yapılandırılmıştır. Bir projeyi yerelinizde çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/EsraKaraduman/softito_projects.git
cd softito_projects
```

### 2. Veritabanı Yapılandırması (SQL Server kullanan projeler için)
* İlgili projenin klasörüne gidin ve `appsettings.json` dosyasını bulun.
* `ConnectionStrings` altındaki `DefaultConnection` bağlantı dizesini kendi SQL Server adresinizle güncelleyin.

### 3. Veritabanını Güncelleyin (Entity Framework Core)
Visual Studio içindeki **Package Manager Console** (Paket Yöneticisi Konsolu) ekranını açın ve şu komutu çalıştırın:
```bash
Update-Database
```
*(Katmanlı mimarili projelerde varsayılan projeyi `Data` veya `Infrastructure` katmanı olarak seçtiğinizden emin olun.)*

### 4. Projeyi Başlatın
Visual Studio'da projeyi açın ve üst menüdeki yeşil **Oynat (Play)** butonuna tıklayarak yerel sunucuda (`localhost`) çalıştırın.

---

👨‍💻 **Geliştirici:** [Esra Karaduman](https://github.com/EsraKaraduman)  
🎓 **Eğitim Kurumu:** Softito Akademi
