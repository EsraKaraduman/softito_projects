namespace CikolataciMVC.Models
{
    public class Kategoriler
    {
        public int Id { get; set; }
        public string KategoriAdi { get; set; }

        public ICollection<Urunler>? Urunler { get; set; }
    }
}
