using Microsoft.EntityFrameworkCore;

namespace MarketMvcProject.Models
{
    public class Urun
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }

        [Precision(18, 2)]
        public decimal Fiyat { get; set; }

        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; }
    }
}

