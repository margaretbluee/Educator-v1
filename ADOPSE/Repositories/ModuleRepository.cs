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

    public IEnumerable<Module> GetModuleStacks(int stackPointer)
    {
        return _aspNetCoreNTierDbContext.Module.Skip(stackPointer * 10 - 10).Take(10); 
    }
}