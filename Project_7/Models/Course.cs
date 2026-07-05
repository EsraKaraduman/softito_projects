namespace Project_7.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Duration { get; set; } = string.Empty;
        public int? InstructorId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Instructor? Instructor { get; set; }
    }
}
