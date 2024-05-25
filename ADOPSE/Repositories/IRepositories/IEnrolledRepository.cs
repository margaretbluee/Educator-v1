using ADOPSE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Repositories.IRepositories;

public interface IEnrolledRepository
{
    public IEnumerable<Module> GetEnrolmentsById(int studentId);
    
    public IEnumerable<object> GetEnrolmentsByUserId(int studentId);

    public Enrolled UpdateEnrolmentCheckboxState(int studentId, int moduleId);

    public bool EnrolmentExist(int studentId, int moduleId);

    public void AddEnrolment(int studentId, int moduleId);

    public bool isEnrolled(int studentId, int moduleId);

    public IEnumerable<object> GetIsEnrolledById(int studentId, int[] moduleIds);

    public int GetModuleCountEnrolledFiltered(Dictionary<string, string> dic, int studentId);

    public IEnumerable<Module> GetFilteredEnrolledModules(Dictionary<string, string> dic, int limit, int offset, int studentId);

}