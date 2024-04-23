using ADOPSE.Models;
 
 namespace ADOPSE.Repositories.IRepositories;
 
 public interface IModuleRepository
 {
  //  string ReturnDescription(string name);
    //public static async Task<string> GenerateRandomDescription(string moduleName);
        bool Save();
    bool Update(Module module);
     IEnumerable<Module> GetModules();
 
     Module GetModuleById(int id);
 
     IEnumerable<Module> GetModuleStacks(int limit, int offset);
 
     int GetModuleCount();
 
     int GetModuleCountByLecturerId(int id);
 


     int GetModuleCountFilteredLucene(Dictionary<string, string> dic);
 

      IEnumerable<Module> GetFilteredModulesLucene(Dictionary<string, string> dic, int limit, int offset);
      


     Module GetModuleByCalendarId(string id);
 
     IEnumerable<Module> GetModuleStacksByLecturerId(int limit, int offset, int id);
 
     public void CreateIndexLucene();
 
}