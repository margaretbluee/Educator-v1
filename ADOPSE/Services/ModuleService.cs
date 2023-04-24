
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADOPSE.Services;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;

    public ModuleService(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
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
}