using System.ComponentModel.DataAnnotations;

namespace Dondurmacf.Model
{
    public class Urun
    {
        [Key]
        public int UrunNo { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }
}
