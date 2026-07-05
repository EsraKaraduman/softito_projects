using System.ComponentModel.DataAnnotations;

namespace SimakYolSupurge.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Müşteri adı alanı zorunludur.")]
        [Display(Name = "Müşteri Adı / Ünvanı")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon Numarası")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Talep edilen araç modeli zorunludur.")]
        [Display(Name = "Talep Edilen Araç Modeli")]
        public string VehicleModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Talep türü zorunludur.")]
        [Display(Name = "Talep Türü")]
        public string RequestType { get; set; } = "Kiralama";

        [Required(ErrorMessage = "Talep tarihi zorunludur.")]
        [Display(Name = "Talep Tarihi")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Müşteri Notları")]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Talep durumu zorunludur.")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Beklemede";
    }
}
