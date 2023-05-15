using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface IModuleRepository
{
    IEnumerable<Module> GetModules();

    Module GetModuleById(int id);

    IEnumerable<Module> GetModuleStacks(int limit, int offset);

    int GetModuleCount();

    IEnumerable<Module> GetFilteredModules(FormattableString query);

    Module GetModuleByCalendarId(string id);


}