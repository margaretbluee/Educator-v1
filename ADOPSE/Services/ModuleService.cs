
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

    public IActionResult GetFilteredModules(Dictionary<string, string> dic,int limit, int offset)
    {
        List<FormattableString> myLista = new List<FormattableString>();

        foreach(KeyValuePair<string, string> ele1 in dic)
        {
            myLista.Add($"{ele1.Key} = {ele1.Value}");
        }
        FormattableString joinedString = myLista.Aggregate((current, next) => $"{current} and {next}");

        FormattableString query = $"select * from Module where {joinedString}";
        
        IEnumerable<Module> modulees = _moduleRepository.GetFilteredModules(query,limit,offset);
        var count = modulees.Count();
        var response = new { count, modules = modulees };
        return new JsonResult(response);
    }

    public Module GetModuleByCalendarId(string id)
    {
        return _moduleRepository.GetModuleByCalendarId(id);
    }
}