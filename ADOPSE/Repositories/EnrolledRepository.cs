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

    public bool isEnrolled(int studentId, int moduleId)
    {
        var enrollment = _aspNetCoreNTierDbContext.Enrolled
            .FirstOrDefault(r => r.StudentId == studentId && r.ModuleId == moduleId);

        return enrollment != null;
    }

    public IEnumerable<object> GetIsEnrolledById(int studentId, int[] moduleIds)
    {
        var enrollments = _aspNetCoreNTierDbContext.Enrolled
            .Where(r => r.StudentId == studentId && moduleIds.Contains(r.ModuleId))
            .Select(r => new { moduleId = r.ModuleId, isEnrolled = true })
            .ToList();

        foreach (var moduleId in moduleIds.Except(enrollments.Select(e => e.moduleId)))
        {
            enrollments.Add(new { moduleId, isEnrolled = false });
        }

        return enrollments;
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

    public IQueryable<Module> QueryEnrolledFiltered(Dictionary<string, string> dic, int studentId)
    {
        var query = _aspNetCoreNTierDbContext.Module
            .Include(m => m.Lecturer)
            .Include(m => m.ModuleType)
            .Where(module => _aspNetCoreNTierDbContext.Enrolled
                .Any(enrolled => enrolled.StudentId == studentId && enrolled.ModuleId == module.Id))
            .AsQueryable();

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

    public int GetModuleCountEnrolledFiltered(Dictionary<string, string> dic, int studentId)
    {
        var toReturn = QueryEnrolledFiltered(dic, studentId).Count();
        return toReturn;
    }

    public IEnumerable<Module> GetFilteredEnrolledModules(Dictionary<string, string> dic, int limit, int offset, int studentId)
    {
        var toReturn = QueryEnrolledFiltered(dic, studentId).Skip(offset).Take(limit).OrderBy(m => m.Id).ToList();
        return toReturn;
    }

}