using System;

namespace Project_7.Models
{
    public class EnrollmentDetailViewModel
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public decimal CoursePrice { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active";
    }
}
