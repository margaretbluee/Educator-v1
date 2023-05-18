using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private readonly ILogger<CalendarController> _logger;

    public CalendarController(ICalendarService calendarService, ILogger<CalendarController> logger)
    {
        _calendarService = calendarService;
        _logger = logger;
    }

    [HttpPost("addModules")]
    public IActionResult Add([FromBody] List<List<string>> lista)
    {
        //Module module = _moduleService.GetModuleByCalendarId()
        /*lista.ForEach(item => item.ForEach(
            item => _logger.LogInformation(item)
            ));
            */
        
        _calendarService.AddEvents(lista);
        

        return Ok();
    }
}