using System.Net;
using System.Security.Claims;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _googleCalendarService;
    private readonly ICalendarService _calendarService;
    private readonly IEventService _eventService;
    private readonly ILogger<CalendarController> _logger;
   
    public CalendarController(ICalendarService calendarService, IEventService eventService, ILogger<CalendarController> logger,
        ICalendarService googleCalendarService)
    {
        _googleCalendarService = googleCalendarService;
        _calendarService = calendarService;
        _eventService = eventService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("fetchAndSync")]
    public IActionResult FetchAndSync()
    {
        List<List<string>> allEvents = _googleCalendarService.RetrieveAllEventsFromGoogleApi();

        // delete unsupported events - refresh updated events - add new events
        _eventService.Sync(allEvents);
        
        return new JsonResult(allEvents);
    }

    [HttpGet("getEvent/{googleEventId}")]
    public ActionResult<Event> getEventByGoogleCalendarEventId(string googleEventId)
    {
        Event eventToRetrieve =  _eventService.GetEventByGoogleCalendarEventId(googleEventId);
        if(eventToRetrieve == null)
        {
            return NotFound();
        }
        _logger.LogInformation("We got the event named -> " + eventToRetrieve.Name + " with eventId " + eventToRetrieve.GoogleCalendarID);

        return eventToRetrieve;
    }

    [Authorize]
    [HttpGet]
    public IEnumerable<Models.Event> GetMyEvents()
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