using ADOPSE.Models;
using ADOPSE.Services;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class ModuleController : ControllerBase
{
    private readonly ILogger<ModuleController> _logger;
    private readonly IModuleService _moduleService;
    private readonly IGoogleCalendarService _googleCalendarService;

    public ModuleController(ILogger<ModuleController> logger, IModuleService moduleService, IGoogleCalendarService googleCalendarService)
    {
        _logger = logger;
        _moduleService = moduleService;
        _googleCalendarService = googleCalendarService;
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
        [FromQuery] string? Rating, [FromQuery] string? Price, [FromQuery] string? SearchQuery, int limit, int offset)
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

        var modules = _moduleService.GetFilteredModules(myDict1, limit, offset);
        return modules;
    }

    [HttpPost("index")]
    public IActionResult CreateIndex()
    {
        _moduleService.CreateIndex();
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

        string googleCalendarId = _googleCalendarService.CreateCalendar(summaryText, descriptionText);

        var updatedModule = _moduleService.UpdateGoogleCalendarIdOfModuleByModuleId(moduleId, googleCalendarId);

        return updatedModule;
    }
}