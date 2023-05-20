using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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


    public bool isEnrolled(int studentId, int moduleId)
    {
        return _enrolledRepository.isEnrolled(studentId, moduleId);
    }

    public IEnumerable<object> GetIsEnrolledById(int studentId, int[] moduleId)
    {
        return _enrolledRepository.GetIsEnrolledById(studentId, moduleId);
    }

    public void AddEnrolment(int studentId, int moduleId)
    {
        _enrolledRepository.AddEnrolment(studentId, moduleId);
    }

    public IActionResult GetFilteredEnrolledModules(Dictionary<string, string> dic, int limit, int offset, int studentId)
    {
        IEnumerable<Module> modules = _enrolledRepository.GetFilteredEnrolledModules(dic, limit, offset, studentId);
        var count = _enrolledRepository.GetModuleCountEnrolledFiltered(dic, studentId);
        var response = new { count, modules };
        return new JsonResult(response);
    }
}