namespace CikolataciMVC.Models
{
    public class Kullanicilar
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } // Admin / User

        public ICollection<Satislar>? Satislar { get; set; }
    }
}

