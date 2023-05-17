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

    public IQueryable<Module> QueryFiltered(Dictionary<string, string> dic){
        var query = _aspNetCoreNTierDbContext.Module.AsQueryable();
                
        if (dic.ContainsKey("ModuleTypeId"))
        {
            int moduleTypeId;
            if (int.TryParse(dic["ModuleTypeId"], out moduleTypeId))
            {
                query = query.Where(module => module.ModuleTypeId == moduleTypeId);
            }
        }

        if (dic.ContainsKey("DifficultyId"))
        {
            int difficultyId;
            if (int.TryParse(dic["DifficultyId"], out difficultyId))
            {
                query = query.Where(module => module.DifficultyId == difficultyId);
            }
        }

        if (dic.ContainsKey("Rating"))
        {
            int rating;
            if (int.TryParse(dic["Rating"], out rating))
            {
                query = query.Where(module => module.Rating == rating);
            }
        }

        if (dic.ContainsKey("Rating"))
        {
            if (dic.TryGetValue("Rating", out var ratingValue))
            {
                var ratings = ratingValue.Split(',');
                var ratingList = ratings.Select(int.Parse).ToList();
                query = query.Where(module => ratingList.Contains(module.Rating));
            }
        }

        if (dic.ContainsKey("Price"))
        {
            if (dic.TryGetValue("Price", out var priceValue))
            {
                var prices = priceValue.Split(',');
                var priceList = prices.Select(int.Parse).ToList();
                query = query.Where(module => priceList.Contains(module.Price));
            }
        }

        return query;
    }

    public int GetModuleCountFiltered(Dictionary<string, string> dic)
    {
        // _logger.LogInformation(query.ToString());
        var toReturn = QueryFiltered(dic).Count();
        return toReturn;
    }

    public IEnumerable<Module> GetFilteredModules(Dictionary<string, string> dic,int limit, int offset)
    {
        // _logger.LogInformation(query.ToString());
        var toReturn = QueryFiltered(dic).Skip(offset).Take(limit).ToList();
        // var toReturn = _aspNetCoreNTierDbContext.Module.FromSqlRaw(query.ToString()).Skip(offset).Take(limit).ToList();
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