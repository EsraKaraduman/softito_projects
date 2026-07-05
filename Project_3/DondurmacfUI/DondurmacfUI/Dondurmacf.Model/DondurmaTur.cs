using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dondurmacf.Model
{
    public class DondurmaTur
    {
        [Key]
        [DisplayName("Tür no")]
        public int TurNo { get; set; }

        [Required]
        [DisplayName("Tür Adı")]
        public string Tur { get; set; } = string.Empty;

        public int Fiyat { get; set; }

        public int UrunNo { get; set; }

        [ForeignKey("UrunNo")]
        public Urun? Urun { get; set; }

        public int? EkleyenKullaniciNo { get; set; }

        [ForeignKey("EkleyenKullaniciNo")]
        public Kullanici? EkleyenKullanici { get; set; }
    }
}
