using ADOPSE.Models;

namespace ADOPSE.Services.IServices;

public interface IEnrolledService
{
    public IEnumerable<Module> GetEnrolmentsById(int studentId);
}