using System.Linq.Expressions;
using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;

namespace ADOPSE.Repositories;

public class LecturerRepository : ILecturerRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    public LecturerRepository(MyDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }

    public IEnumerable<Lecturer> GetAllLecturers()
    {
        return _aspNetCoreNTierDbContext.Lecturer.ToList();
    }

    public Lecturer GetLecturerById(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateLecturer(Lecturer lecturer)
    {
        throw new NotImplementedException();
    }

    public void AddLecturer(Lecturer lecturer)
    {
        throw new NotImplementedException();
    }
}