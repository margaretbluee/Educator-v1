using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    private readonly ILogger<ModuleRepository> _logger;

    public ModuleRepository(MyDbContext aspNetCoreNTierDbContext, ILogger<ModuleRepository> logger)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
        _logger = logger;
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

    public IEnumerable<Module> GetFilteredModules(FormattableString query)
    {
        _logger.LogInformation(query.ToString());
        var toReturn = _aspNetCoreNTierDbContext.Module.FromSqlRaw(query.ToString()).ToList();
        toReturn.ForEach((module => _logger.LogInformation(module.Name)));
        return toReturn;
    }

    public Module GetModuleByCalendarId(string id)
    {
        return _aspNetCoreNTierDbContext.Module.Where(module => module.GoogleCalendarID.Equals(id)).FirstOrDefault();
    }

    public IEnumerable<Module> GetModuleStacks(int limit, int offset)
    {
        return _aspNetCoreNTierDbContext.Module.Skip(offset).Take(limit);
    }
}