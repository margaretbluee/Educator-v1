using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Repositories;

public class EnrolledRepository : IEnrolledRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    private readonly ILogger<EnrolledRepository> _logger;

    public EnrolledRepository(MyDbContext aspNetCoreNTierDbContext, ILogger<EnrolledRepository> logger)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
        _logger = logger;
    }

    public IEnumerable<Module> GetEnrolmentsById(int studentId)
    {
        var enrollments = _aspNetCoreNTierDbContext.Enrolled.FromSql($"select * from Enrolled where studentId = {studentId}").Select(r => r.Module.Id).ToList();
        enrollments.ForEach(x => _logger.LogInformation($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA Repository list {x}"));
        //return _aspNetCoreNTierDbContext.Module.FromSql($"select * from Module").ToList();
        return _aspNetCoreNTierDbContext.Module.Where(x => enrollments.Contains(x.Id)).ToList();
    }

    public void AddEnrolment(int studentId, int moduleId)
    {
        _aspNetCoreNTierDbContext.Enrolled.Add(
            new Enrolled
            {
                Module = _aspNetCoreNTierDbContext.Module.Where(x => x.Id == moduleId).First(),
                Student = _aspNetCoreNTierDbContext.Student.Where(x => x.Id == studentId).First(),
            }
        );

        _aspNetCoreNTierDbContext.SaveChanges();
    }
}