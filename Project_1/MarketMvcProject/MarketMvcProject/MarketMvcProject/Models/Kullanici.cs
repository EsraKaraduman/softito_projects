using System.ComponentModel.DataAnnotations;

namespace MarketMvcProject.Models
{
    public class Kullanici
    {
        public int KullaniciId { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string Sifre { get; set; }

        [Required]
        public string Rol { get; set; }  
    }
}
