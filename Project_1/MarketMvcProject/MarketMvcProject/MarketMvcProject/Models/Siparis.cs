namespace MarketMvcProject.Models
{
    public class Siparis
    {
        public int SiparisId { get; set; }
        public DateTime Tarih { get; set; }

        public int MusteriId { get; set; }
        public Musteri? Musteri { get; set; }

        public int UrunId { get; set; }
        public Urun? Urun { get; set; }
    }
}
