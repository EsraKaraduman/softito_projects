using System.Collections.Generic;
using System.Threading.Tasks;
using Project_7.Models;

namespace Project_7.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<int> EnrollAsync(int userId, int courseId);
        Task<IEnumerable<EnrollmentDetailViewModel>> GetEnrollmentsAsync(int? userId);
        Task<bool> CancelEnrollmentAsync(int enrollmentId);
    }
}
