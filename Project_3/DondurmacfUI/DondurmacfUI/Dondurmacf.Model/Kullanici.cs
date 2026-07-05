using System.ComponentModel.DataAnnotations;

namespace Dondurmacf.Model
{
    public class Kullanici
    {
        [Key]
        public int KullaniciNo { get; set; }

        [Required]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        public string Sifre { get; set; } = string.Empty;

        [Required]
        public string AdSoyad { get; set; } = string.Empty;
    }
}
