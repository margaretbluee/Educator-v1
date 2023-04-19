using ADOPSE.Models;

namespace ADOPSE.Services.IServices;

public interface IModuleService
{
    IEnumerable<Module> GetModules();

    Module GetModuleById(int id);
    
    IEnumerable<Module> GetModuleStacks(int stackPointer);


}