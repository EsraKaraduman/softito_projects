# Project 5: Depo & Stok Takip Sistemi (StockManager)

Bu proje, işletmelerin depolarındaki ürünleri, tedarikçileri ve depo içi ürün hareketlerini yönetmek amacıyla geliştirilmiş **ASP.NET Core MVC** tabanlı bir **Stok Takip Sistemi**dir.

## 💻 Teknolojiler
* **Framework:** ASP.NET Core MVC (v8.0)
* **Veritabanı:** MS SQL Server & Entity Framework Core (Code-First)
* **Tasarım:** HTML5, CSS3, Bootstrap, Javascript, Chart.js (Grafikler için)

## 🚀 Özellikler
* **Gösterge Paneli (DashboardController):** Toplam ürün adedi, aktif tedarikçi sayısı, kritik stok uyarıları ve depo doluluk oranlarının görselleştirildiği ana ekran.
* **Ürün Yönetimi (ProductController):** Ürünlerin barkod, kategori, fiyat ve mevcut stok bilgileriyle listelenmesi ve yönetilmesi.
* **Tedarikçi Yönetimi (SupplierController):** Ürün satın alınan firmaların veya kişilerin iletişim ve tedarik bilgileri.
* **Depo Yönetimi (WarehouseController):** Farklı fiziksel depoların/rafların tanımlanması ve ürünlerin depolara göre dağılımı.
* **Stok Raporları (ReportController):** Stok hareket günlükleri (giriş-çıkış işlemleri), en çok hareket gören ürünler ve stok maliyeti analizleri.
* **Kimlik Doğrulama (AccountController):** Güvenli oturum açma işlemleri.

## 📸 Ekran Görüntüleri

### Stok Kontrol Paneli ve Ürün Envanteri
<p align="center">
  <img src="../assets/StockManager_image_1.png" width="48%" alt="Ana Dashboard" />
  <img src="../assets/StockManager_image_2.png" width="48%" alt="Ürün Listesi" />
</p>

<details>
  <summary>🔍 Diğer Ekran Görüntülerini Göster</summary>
  <br>
  <p align="center">
    <img src="../assets/StockManager_image_3.png" width="48%" alt="Yeni Ürün Kaydı" />
    <img src="../assets/StockManager_image_4.png" width="48%" alt="Tedarikçi Listesi" />
  </p>
  <p align="center">
    <img src="../assets/StockManager_image_5.png" width="48%" alt="Depo Listesi" />
    <img src="../assets/StockManager_image_6.png" width="48%" alt="Stok Giriş-Çıkış Hareketi" />
  </p>
  <p align="center">
    <img src="../assets/StockManager_image_7.png" width="48%" alt="Detaylı Raporlama" />
    <img src="../assets/StockManager_image_8.png" width="48%" alt="Stok Çıkış İşlemleri" />
  </p>
  <p align="center">
    <img src="../assets/StockManager_image_9.png" width="48%" alt="Giriş Ekranı" />
    <img src="../assets/StockManager_image_10.png" width="48%" alt="Kayıt Sayfası" />
  </p>
  <p align="center">
    <img src="../assets/StockManager_image_11.png" width="48%" alt="Tedarikçi Detayı" />
  </p>
</details>

## 📂 Dosya Yapısı
* `Controllers/`: Stok, tedarik ve depo süreçlerini yöneten kontrolcüler.
* `Models/`: Product, Supplier, Warehouse, StockMovement ve User veri modelleri.
* `Views/`: Depo kontrol paneli, ürün listeleri ve tedarikçi formları.
* `Data/`: Entity Framework Context ve Migrations.

