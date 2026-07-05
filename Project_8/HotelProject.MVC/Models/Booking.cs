namespace HotelProject.MVC.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; } = "Confirmed";
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
    }
}
