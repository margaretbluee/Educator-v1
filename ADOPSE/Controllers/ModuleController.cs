using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class ModuleController : ControllerBase
{
    private readonly ILogger<ModuleController> _logger;
    private readonly IModuleService _moduleService;
    private readonly IElasticClient _elasticClient;
    private readonly IModuleRepository _moduleRepository;
    private readonly ILuceneRepository _luceneRepository;
    private readonly ICalendarService _googleCalendarService;


    public ModuleController(ILogger<ModuleController> logger, IModuleService moduleService, IElasticClient elasticClient, ICalendarService googleCalendarService)
    {
        _logger = logger;
        _moduleService = moduleService;
        _elasticClient = elasticClient;
        _googleCalendarService = googleCalendarService;
    }


    //update with Custom Google Search Api
    [HttpPost("update/{from}/{to}")]
    public async Task<IActionResult> UpdateModulesWithDescription(int from, int to)
    {
        bool success = await _moduleService.UpdateModulesWithGoogleDescription(from, to);

        if (success)
        {
            return Ok("Modules updated successfully.");
        }
        else
        {
            return BadRequest("Failed to update modules. Check if all modules exist and update was successful.");
        }
    }


    [HttpPut("{id}/fixDescription")]
    public async Task<IActionResult> UpdateModuleWithFaultyDescription(int id)
    {
        var result = await _moduleService.FixWrongDescriptions(id);
        if (result)
        {
            return Ok("Faulty description updated successfully.");
        }
        else
        {
            return NotFound("Module not found.");
        }
    }
    //Descriptions with LLM MistralAI
    [HttpPut("mistral/{from}/{to}")]
    public async Task<IActionResult> Mistral_test(int from, int to)
    {
        var result = await _moduleService.Mistral(from, to);
        if (result)
        {
            return Ok("MISTRAL descriptions updated successfully.");
        }
        else
        {
            return NotFound("Module not found.");
        }
    }

    //write files with missing descriptions(DEBUG)
    [HttpPut("Find_lost_Descriptions")]
    public IActionResult Find_lost_descriptions()
    {
        bool result = _moduleService.Find_missing_files();

        if (result)
        {
            return Ok("Missing IDs have been written to missing_ids.txt.");
        }
        else
        {
            return BadRequest("Failed to write missing IDs.");
        }
    }

    //update a single module using google search, and the sixth link
    [HttpPut("{id}/google_Description")]
    public async Task<IActionResult> UpdateModuleWithRandomDescription(int id)
    {
        var result = await _moduleService.UpdateModuleWithFaultyDescription(id);
        if (result)
        {
            return Ok("Random description updated successfuxlly.");
        }
        else
        {
            return NotFound("Module not found.");
        }
    }



    [HttpGet]
    public IEnumerable<Module> Get()
    {
        return _moduleService.GetModules();
    }

    [HttpGet("{id}")]
    public ActionResult<Module> GetModule(int id)
    {
        Module module = _moduleService.GetModuleById(id);
        if (module == null)
            return NotFound();
        _logger.LogInformation("AAAAAAAAAAAAAAAAAAAAAA -> " + module.Name);
        return _moduleService.GetModuleById(id);
    }

    [HttpGet("stack/{limit}/{offset}")]
    public IActionResult GetStack(int limit, int offset)
    {
        return _moduleService.GetModuleStacks(limit, offset);
    }

    [HttpGet("lecturer/{id}/{limit}/{offset}")]
    public IActionResult GetStackByLecturerId(int limit, int offset, int id)
    {
        return _moduleService.GetModuleStacksByLecturerId(limit, offset, id);
    }

    [HttpGet("filtered/{limit}/{offset}")]
    public IActionResult GetFilteresModules([FromQuery] string? ModuleTypeId, [FromQuery] string? DifficultyId,
        [FromQuery] string? Rating, [FromQuery] string? Price, [FromQuery] string? SearchQuery,int limit, int offset)
    {
        _logger.LogInformation(ModuleTypeId);
        _logger.LogInformation(DifficultyId);
        _logger.LogInformation(Rating);

        Dictionary<string, string> myDict1 = new Dictionary<string, string>();

        if (ModuleTypeId != null)
            myDict1.Add("ModuleTypeId", ModuleTypeId);
        if (DifficultyId != null)
            myDict1.Add("DifficultyId", DifficultyId);
        if (Rating != null)
            myDict1.Add("Rating", Rating);
        if (Price != null)
            myDict1.Add("Price", Price);
        if (SearchQuery != null)
            myDict1.Add("SearchQuery", SearchQuery);

        var modules = _moduleService.GetFilteredModulesLucene(myDict1, limit, offset);

        return modules;
    }


    /*   [HttpGet("search")]
     public IActionResult SearchModules([FromQuery] string searchQuery)
     {
         try
         {
             var modules = _luceneRepository.SearchModulesElastic(searchQuery);
             return Ok(modules);
         }
         catch (Exception ex)
         {
             return StatusCode(500, $"An error occurred: {ex.Message}");
         }
     }*/

    [HttpPost("index_elastic")]
    public async Task<IActionResult> Create_ELS_Index()
    {
        try
        {
            _luceneRepository.CreateIndexElastic();
            return Ok("Index created successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("index")]
    public IActionResult CreateIndex()
    {
        _moduleService.CreateIndexLucene();
        return Ok(); // Or return appropriate status code and response
    }

    [HttpPut("{moduleId}/googleCalendarId")]
    public ActionResult<Module> UpdateGoogleCalendarIdOfModule(int moduleId)
    {
        var exists = _moduleService.ExistsModuleById(moduleId);
        if (!exists)
        {
            return NotFound(new { message = $"Module with id '{moduleId}' does not exist" });
        }

        var isEmpty = _moduleService.IsGoogleCalendarIdEmpty(moduleId);
        if (!isEmpty)
        {
            return BadRequest(new { message = $"Module with id '{moduleId}' arleady has a its own calendar!" });
        }

        var module = _moduleService.GetModuleById(moduleId);
        var summaryText = module.Name;
        var descriptionText = module.Description;
        string googleCalendarId = "";
        try{
            googleCalendarId = _googleCalendarService.CreateCalendar(summaryText, descriptionText);
        }catch(Google.GoogleApiException ex){
           // Console.WriteLine("We lost access to google calendar api due to request limit, " + ex.Message);
            return BadRequest(new { message = "An error occurred while accessing Google API. Please try again later" });
        }

        var updatedModule = _moduleService.UpdateGoogleCalendarIdOfModuleByModuleId(moduleId, googleCalendarId);

        return updatedModule;
    }
}