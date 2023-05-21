using ADOPSE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Services.IServices;

public interface IEnrolledService
{
    public IEnumerable<Module> GetEnrolmentsById(int studentId);

    public void AddEnrolment(int studentId, int moduleId);

    public bool isEnrolled(int studentId, int moduleId);

    public IEnumerable<object> GetIsEnrolledById(int studentId, int[] moduleId);

    public IActionResult GetFilteredEnrolledModules(Dictionary<string, string> dic, int limit, int offset, int studentId);
}