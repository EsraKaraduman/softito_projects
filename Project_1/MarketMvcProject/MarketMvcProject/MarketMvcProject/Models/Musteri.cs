namespace MarketMvcProject.Models
{
    public class Musteri
    {
        public int MusteriId { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }

        public List<Siparis>? Siparisler { get; set; }
    }
}
