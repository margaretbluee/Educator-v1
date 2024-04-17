using System.Security.Claims;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IModuleService _moduleService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventService eventService, ILogger<EventsController> logger, IModuleService moduleService)
    {
        _eventService = eventService;
        _logger = logger;
        _moduleService = moduleService;
    }

    [HttpGet("{id}")]
    public IEnumerable<Event> GetEventsByModuleId(int id)
    {
        return _eventService.GetEventsByModuleId(id);
    }

    // STH WRONG IN HERE... Module might has more than one event
    [HttpGet("module/{moduleId}")]
    public IActionResult GetGoogleCalendarEventIdByModuleId(int moduleId)
    {
        bool moduleExists = _moduleService.ExistsModuleById(moduleId);
        if (!moduleExists)
        {
            return NotFound(new { message = "The module with Id: " + moduleId + " does not exist" });
        }
        var eventToReturn = _eventService.GetEventByModuleId(moduleId);

        var googleCalendarId = eventToReturn.Module.GoogleCalendarID;
        if( String.IsNullOrEmpty(googleCalendarId))
        {
            return NotFound(new { message = "The module with Id: " + moduleId + " does not match to a calendar" });
        }        

        return new JsonResult(new { GoogleCalendarId = googleCalendarId });
    }

    //[HttpGet("module/id/{moduleId}")]
    //public IActionResult GetGoogleCalendarIdByModuleId(int moduleId)
    //{
    //    bool moduleExists = _moduleService.ExistsModuleById(moduleId);
    //    if (!moduleExists)
    //    {
    //        return NotFound(new { message = "The module with Id: " + moduleId + " does not exists" });
    //    }
    //    var googleCalendarId = _eventService.GetGoogleCalendarIdByModuleId(moduleId);

    //    //if (String.IsNullOrEmpty(googleCalendarId))
    //    //{
    //    //    return NotFound(new { message = "The module with Id: " + moduleId + " does not match to a calendar" });
    //    //}

    //    return new JsonResult(new { GoogleCalendarId = googleCalendarId });
    //}

}