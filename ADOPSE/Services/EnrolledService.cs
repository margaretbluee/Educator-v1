using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;

namespace ADOPSE.Services;

public class EnrolledService : IEnrolledService
{
    private readonly IEnrolledRepository _enrolledRepository;

    public EnrolledService(IEnrolledRepository enrolledRepository)
    {
        _enrolledRepository = enrolledRepository;
    }

    public IEnumerable<Module> GetEnrolmentsById(int studentId)
    {
        return _enrolledRepository.GetEnrolmentsById(studentId);
    }

    public void AddEnrolment(int studentId, int moduleId)
    {
        _enrolledRepository.AddEnrolment(studentId,moduleId);
    }
}