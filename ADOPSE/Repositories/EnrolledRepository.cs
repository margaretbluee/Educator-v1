using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;

namespace ADOPSE.Repositories;

public class EnrolledRepository : IEnrolledRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;

    public EnrolledRepository(MyDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }


    public IEnumerable<Module> GetEnrolmentsById(int studentId)
    {
        return _aspNetCoreNTierDbContext.Module.Where(x => x.Id == 15183).ToList();
    }
}