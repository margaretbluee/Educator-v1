using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly IModuleService _moduleService;
    private readonly ILogger<CalendarController> _logger;

    public CalendarController(IModuleService moduleService, ILogger<CalendarController> logger)
    {
        _moduleService = moduleService;
        _logger = logger;
    }

    [HttpPost("addModules")]
    public IActionResult Add([FromBody] List<string> lista)
    {
        //Module module = _moduleService.GetModuleByCalendarId()
        lista.ForEach(item => _logger.LogInformation(item));

        return Ok();
    }
}