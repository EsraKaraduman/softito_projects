using System.ComponentModel.DataAnnotations;

namespace SimakYolSupurge.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka alanı zorunludur.")]
        [Display(Name = "Marka")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model alanı zorunludur.")]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tür alanı zorunludur.")]
        [Display(Name = "Tür / Sınıf")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yıl alanı zorunludur.")]
        [Range(1900, 2100, ErrorMessage = "Geçerli bir yıl giriniz.")]
        [Display(Name = "Model Yılı")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Durum alanı zorunludur.")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Kiralık";

        [Display(Name = "Görsel URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;
    }
}
