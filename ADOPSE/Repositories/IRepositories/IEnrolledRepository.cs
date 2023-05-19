using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface IEnrolledRepository
{
    public IEnumerable<Module> GetEnrolmentsById(int studentId);

    public void AddEnrolment(int studentId, int moduleId);

    public bool isEnrolled(int studentId, int moduleId);

    public IEnumerable<object> GetIsEnrolledById(int studentId, int[] moduleIds);
}