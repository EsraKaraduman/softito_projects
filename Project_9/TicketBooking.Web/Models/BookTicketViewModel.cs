using System;
using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Web.Models;

public class BookTicketViewModel
{
    public int EventId { get; set; }
    
    public string EventTitle { get; set; } = string.Empty;
    public string EventLocation { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }

    [Required(ErrorMessage = "Bilet adeti zorunludur.")]
    [Range(1, 10, ErrorMessage = "Tek seferde en az 1, en fazla 10 bilet alabilirsiniz.")]
    [Display(Name = "Bilet Adeti")]
    public int Quantity { get; set; } = 1;

    [Required(ErrorMessage = "Koltuk seçimi/numarası zorunludur.")]
    [Display(Name = "Koltuk Numarası")]
    public string SeatNumber { get; set; } = "A-1";
}
