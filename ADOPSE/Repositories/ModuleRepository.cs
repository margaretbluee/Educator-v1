using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

namespace ADOPSE.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    private readonly ILogger<ModuleRepository> _logger;

    private readonly ILuceneRepository _luceneRepository;


    public bool Update(Module module)
    {
        _aspNetCoreNTierDbContext.Update(module);
        return Save();
    }
    public bool Save()
    {
        var saved = _aspNetCoreNTierDbContext.SaveChanges();
        return saved > 0 ? true : false;
    }



    public ModuleRepository(MyDbContext aspNetCoreNTierDbContext, ILogger<ModuleRepository> logger, ILuceneRepository luceneRepository)
    {
        _luceneRepository = luceneRepository;
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
        _logger = logger;

    }

    public IEnumerable<Module> GetModules()
    {
        return _aspNetCoreNTierDbContext.Module.Take(10).ToList();
    }

    public Module GetModuleById(int id)
    {
        return _aspNetCoreNTierDbContext.Module.Include(m => m.Lecturer).Include(m => m.ModuleType).Where(x => x.Id == id).FirstOrDefault();
    }



    public IQueryable<Module> QueryFilteredLucene(Dictionary<string, string> dic)
    {

        var query = _aspNetCoreNTierDbContext.Module.Include(m => m.Lecturer).Include(m => m.ModuleType).AsQueryable();

        if (dic.ContainsKey("SearchQuery"))
        {
            string searchQuery;
            if (dic.TryGetValue("SearchQuery", out searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                IEnumerable<Module> searchResults = _luceneRepository.SearchModulesLucene(searchQuery);
                _logger.LogInformation($"Search Query: {searchQuery}");

                _logger.LogInformation($"Search Results Count: {searchResults?.Count() ?? 0}");

                var searchResultIds = searchResults.Select(searchModule => Convert.ToInt32(searchModule.Id));

                query = query.Where(module => searchResultIds.Contains(module.Id));
            }
        }

        if (dic.ContainsKey("ModuleTypeId"))
        {
            int moduleTypeId;
            if (int.TryParse(dic["ModuleTypeId"], out moduleTypeId) && moduleTypeId != 0)
            {
                query = query.Where(module => module.ModuleTypeId == moduleTypeId);
            }
        }

        if (dic.ContainsKey("DifficultyId"))
        {
            int difficultyId;
            if (int.TryParse(dic["DifficultyId"], out difficultyId) && difficultyId != 0)
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
                if (ratings.Length == 2 && int.TryParse(ratings[0], out int minRating) && int.TryParse(ratings[1], out int maxRating))
                {
                    query = query.Where(module => module.Rating >= minRating && module.Rating <= maxRating);
                }
            }
        }

        if (dic.ContainsKey("Price"))
        {
            if (dic.TryGetValue("Price", out var priceValue))
            {
                var prices = priceValue.Split(',');
                if (prices.Length == 2 && int.TryParse(prices[0], out int minPrice) && int.TryParse(prices[1], out int maxPrice))
                {
                    query = query.Where(module => module.Price >= minPrice && module.Price <= maxPrice);
                }
            }
        }

        return query;
    }

    public int GetModuleCountFilteredLucene(Dictionary<string, string> dic)
    {
        // _logger.LogInformation(query.ToString());
        var toReturn = QueryFilteredLucene(dic).Count();
        return toReturn;
    }

    public IEnumerable<Module> GetFilteredModulesLucene(Dictionary<string, string> dic, int limit, int offset)
    {
        // _logger.LogInformation(query.ToString());
        var toReturn = QueryFilteredLucene(dic).Skip(offset).Take(limit).OrderBy(m => m.Id).ToList();
        // var toReturn = _aspNetCoreNTierDbContext.Module.FromSqlRaw(query.ToString()).Skip(offset).Take(limit).ToList();
        // toReturn.ForEach((module => _logger.LogInformation(module.Name)));
        return toReturn;
    }

    public Module GetModuleByCalendarId(string id)
    {
        return _aspNetCoreNTierDbContext.Module.Where(module => module.GoogleCalendarID.Equals(id)).FirstOrDefault();
    }

    public int GetModuleCount()
    {
        return _aspNetCoreNTierDbContext.Module.Count();
    }
    public IEnumerable<Module> GetModuleStacks(int limit, int offset)
    {
        return _aspNetCoreNTierDbContext.Module.Skip(offset).Take(limit).OrderBy(m => m.Id);
    }

    public int GetModuleCountByLecturerId(int id)
    {
        return _aspNetCoreNTierDbContext.Module.Where(x => x.leaderId == id).Count();
    }
    public IEnumerable<Module> GetModuleStacksByLecturerId(int limit, int offset, int id)
    {
        return _aspNetCoreNTierDbContext.Module.Where(x => x.leaderId == id).Skip(offset).Take(limit).OrderBy(m => m.Id);
    }

    public void CreateIndexLucene()
    {
        _luceneRepository.CreateIndexLucene();
    }



    public IEnumerable<string> GetAllCalendarIds()
    {
        return _aspNetCoreNTierDbContext.Module.Where(m => !string.IsNullOrEmpty(m.GoogleCalendarID)).Select(m => m.GoogleCalendarID).ToList();
    }

    public bool ExistsModuleById(int id)
    {
        var module = _aspNetCoreNTierDbContext.Module.Where(m => m.Id == id).FirstOrDefault();
        return module != null;
    }

    public bool IsGoogleCalendarIdEmpty(int moduleId)
    {
        return _aspNetCoreNTierDbContext.Module.Where(m => m.Id == moduleId).Any(m => string.IsNullOrEmpty(m.GoogleCalendarID));
    }

    public Module UpdateGoogleCalendarIdOfModuleByModuleId(int moduleId, string googleCalendarId)
    {
        var moduleToUpdate = _aspNetCoreNTierDbContext.Module.Find(moduleId);

        if (moduleToUpdate != null)
        {
            moduleToUpdate.GoogleCalendarID = googleCalendarId;
        }

        _aspNetCoreNTierDbContext.SaveChanges();

        return moduleToUpdate;
    }

    public bool ClearGoogleCalendarIdOfModuleByCalendarId(string calendarId)
    {
        var moduleToUpdate = _aspNetCoreNTierDbContext.Module.Where(m => m.GoogleCalendarID == calendarId).FirstOrDefault();

        if (moduleToUpdate != null)
        {
            moduleToUpdate.GoogleCalendarID = "";
            _logger.LogInformation($"Module with id: '{moduleToUpdate.Id}' does not have google calendar id from now on");
            _aspNetCoreNTierDbContext.SaveChanges();
            return true;
        }

        _logger.LogError($"Cannot clear the googleCalendar Id '{calendarId}' of a module because the module instance does not exist");
        return false;
    }
}
