using System.ComponentModel.DataAnnotations;

namespace SimakYolSupurge.Models
{
    public class Maintenance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Araç modeli alanı zorunludur.")]
        [Display(Name = "Araç Modeli")]
        public string VehicleModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bakım açıklaması zorunludur.")]
        [Display(Name = "Açıklama / Arıza")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Maliyet alanı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Maliyet 0'dan büyük olmalıdır.")]
        [Display(Name = "Maliyet (TL)")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Durum alanı zorunludur.")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Devam Ediyor";

        [Required(ErrorMessage = "Teknisyen adı zorunludur.")]
        [Display(Name = "Teknisyen / Servis")]
        public string TechnicianName { get; set; } = string.Empty;
    }
}
