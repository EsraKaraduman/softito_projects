namespace CikolataciMVC.Models
{
    public class Satislar
    {
        public int Id { get; set; }

        public int UrunId { get; set; }
        public Urunler? Urun { get; set; }

        public int KullaniciId { get; set; }
        public Kullanicilar? Kullanici { get; set; }

        public int Adet { get; set; }
        public DateTime Tarih { get; set; }
    }
}

