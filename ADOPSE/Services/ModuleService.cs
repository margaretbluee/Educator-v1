
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;

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

    public IEnumerable<Module> GetModuleStacks(int stackPointer)
    {
        return _moduleRepository.GetModuleStacks(stackPointer);
    }
}