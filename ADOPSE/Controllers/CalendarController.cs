using System.Security.Claims;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private readonly IEventService _eventService;
    private readonly ILogger<CalendarController> _logger;

    public CalendarController(ICalendarService calendarService, IEventService eventService, ILogger<CalendarController> logger)
    {
        _calendarService = calendarService;
        _eventService = eventService;
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

    [Authorize]
    [HttpGet]
    public IEnumerable<Event> GetMyEvents()
    {
        int studentId = GetClaimedStudentId();
        return _eventService.GetEventsByStudentId(studentId);
    }
    
    private int GetClaimedStudentId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            var Id = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return Id;
        }
        return -1;
    }
}