using ADOPSE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Services.IServices;

public interface IModuleService
{
    IEnumerable<Module> GetModules();

    Module GetModuleById(int id);

    IActionResult GetModuleStacks(int limit, int offset);

    IEnumerable<Module> GetFilteredModules(Dictionary<string, string> dic);
    
    Module GetModuleByCalendarId(string id);

}