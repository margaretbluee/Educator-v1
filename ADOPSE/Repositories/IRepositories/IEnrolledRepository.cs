using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface IEnrolledRepository
{
    public IEnumerable<Module> GetEnrolmentsById(int studentId);
}