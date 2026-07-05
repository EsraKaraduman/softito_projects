using System.ComponentModel.DataAnnotations.Schema;

namespace CikolataciMVC.Models
{
    public class Urunler
    {
        public int Id { get; set; }
        public string UrunAdi { get; set; 
        }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fiyat { get; set; }

        public int KategoriId { get; set; }
        public Kategoriler? Kategori { get; set; }

        public ICollection<Satislar>? Satislar { get; set; }
    }
}
