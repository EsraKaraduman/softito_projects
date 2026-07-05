using System;

namespace Project_7.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active";
        public User? User { get; set; }
        public Course? Course { get; set; }
    }
}
