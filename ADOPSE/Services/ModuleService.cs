
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;

namespace ADOPSE.Services;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;
    private readonly ILogger<ModuleService> _logger;

    public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
    {
        _moduleRepository = moduleRepository;
        _logger = logger;
        CreateIndex();
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

    public void CreateIndex()
    {
        _moduleRepository.CreateIndex();
    }
}