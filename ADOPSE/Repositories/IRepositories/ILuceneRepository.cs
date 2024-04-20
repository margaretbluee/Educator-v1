using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface ILuceneRepository
{
    public IEnumerable<Module> SearchModules(string searchQuery, int searchType);

    public void CreateIndex();
}