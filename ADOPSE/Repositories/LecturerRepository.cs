using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;

namespace ADOPSE.Repositories;

public class LecturerRepository : GenericRepository<Lecturer>, ILecturerRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    public LecturerRepository(MyDbContext aspNetCoreNTierDbContext) : base(aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }
}