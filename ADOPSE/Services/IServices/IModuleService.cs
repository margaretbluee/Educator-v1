 using ADOPSE.Models;
 using Microsoft.AspNetCore.Mvc;
 
 namespace ADOPSE.Services.IServices;
 
 public interface IModuleService
 {
   Task<bool> UpdateModuleWithFaultyDescription(int id);
   Task<bool> UpdateModulesWithRandomDescription(int start, int end);

 Task<bool> FixWrongDescriptions(int moduleId);
 Task<bool> Mistral (int start, int end);
  bool Find_missing_files();

     IEnumerable<Module> GetModules();
 
     Module GetModuleById(int id);
 
     IActionResult GetModuleStacks(int limit, int offset);
 
     IActionResult GetModuleStacksByLecturerId(int limit, int offset, int id);
 
     IActionResult GetFilteredModulesLucene(Dictionary<string, string> dic, int limit, int offset);
  
     Module GetModuleByCalendarId(string id);
 
     public void CreateIndexLucene();
 
 }