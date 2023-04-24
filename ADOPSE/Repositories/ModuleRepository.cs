using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;

namespace ADOPSE.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;

    public ModuleRepository(MyDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }

    public IEnumerable<Module> GetModules()
    {
        return _aspNetCoreNTierDbContext.Module.Take(10).ToList();
    }

    public Module GetModuleById(int id)
    {
        return _aspNetCoreNTierDbContext.Module.Where(x => x.Id == id).FirstOrDefault();
    }

    public int GetModuleCount()
    {
        return _aspNetCoreNTierDbContext.Module.Count();
    }

    public IEnumerable<Module> GetModuleStacks(int limit, int offset)
    {
        return _aspNetCoreNTierDbContext.Module.Skip(offset).Take(limit);
    }
}