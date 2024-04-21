
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Google.Apis.Calendar.v3.Data;

namespace ADOPSE.Services;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;
    
    private readonly ILogger<ModuleService> _logger;

    public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
    {
        _moduleRepository = moduleRepository;
        _logger = logger;        
    }

    public IEnumerable<Module> GetModules()
    {
        return _moduleRepository.GetModules();
    }

    public Module GetModuleById(int id)
    {
        return _moduleRepository.GetModuleById(id);
    }

    public IActionResult GetModuleStacks(int limit, int offset)
    {
        var modules = _moduleRepository.GetModuleStacks(limit, offset);
        var count = _moduleRepository.GetModuleCount();
        var response = new { count, modules };
        return new JsonResult(response);
    }

    public IActionResult GetModuleStacksByLecturerId(int limit, int offset, int id)
    {
        var modules = _moduleRepository.GetModuleStacksByLecturerId(limit, offset, id);
        var count = _moduleRepository.GetModuleCountByLecturerId(id);
        var response = new { count, modules };
        return new JsonResult(response);
    }

    public IActionResult GetFilteredModules(Dictionary<string, string> dic, int limit, int offset)
    {
        IEnumerable<Module> modules = _moduleRepository.GetFilteredModules(dic, limit, offset);
        var count = _moduleRepository.GetModuleCountFiltered(dic);
        var response = new { count, modules };
        return new JsonResult(response);
    }

    public Module GetModuleByCalendarId(string id)
    {
        return _moduleRepository.GetModuleByCalendarId(id);
    }

    public IEnumerable<string> GetAllCalendarsIds()
    {
        return _moduleRepository.GetAllCalendarIds();
    }

    public void CreateIndex()
    {
        _moduleRepository.CreateIndex();
    }

    public bool ExistsModuleById(int id)
    {
        return _moduleRepository.ExistsModuleById(id);
    }
    public bool IsGoogleCalendarIdEmpty(int moduleId)
    {
        return _moduleRepository.IsGoogleCalendarIdEmpty(moduleId);
    }

    public ActionResult<Module> UpdateGoogleCalendarIdOfModuleByModuleId(int moduleId, string googleCalendarId) 
    {             
            var updatedModule = _moduleRepository.UpdateGoogleCalendarIdOfModuleByModuleId(moduleId, googleCalendarId);

        return new JsonResult(updatedModule);

    }

    public void SyncGoogleCalendarsIdsOfModules(HashSet<string> calendarIdsList)
    {
        _logger.LogInformation($"The number of calendars is: " + calendarIdsList.Count);

        var calendarIds = _moduleRepository.GetAllCalendarIds();

        foreach (var calendarId in calendarIds)
        {
            if (!calendarIdsList.Contains(calendarId))
            {
                _moduleRepository.ClearGoogleCalendarIdOfModuleByCalendarId(calendarId);
            }
        }
        _logger.LogInformation("Calendar Ids Sync has successfully finished");
    }
}