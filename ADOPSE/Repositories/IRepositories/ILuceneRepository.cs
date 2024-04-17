using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface ILuceneRepository
{
    public IEnumerable<Module> SearchModulesLucene(string searchQuery);
  public IEnumerable<Module> SearchModulesElastic(string searchQuery);
    public void CreateIndexLucene();
    
    public void CreateIndexElastic();
}