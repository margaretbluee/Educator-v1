using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Services;

public class EventService : IEventService
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    private readonly ILogger<EventService> _logger;


    public EventService(MyDbContext aspNetCoreNTierDbContext, ILogger<EventService> logger)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
        _logger = logger;
    }

    private void DeleteEvent(string eventGoogleCalendarId)
    {
        _aspNetCoreNTierDbContext.Event.FromSqlRaw(
            $"delete from Event where GoogleCalendarID = '{eventGoogleCalendarId}';");
    }

    public void DeleteAllEvents(HashSet<string> eventsGoogleCalendarIds)
    {
        foreach (var eventsGoogleCalendarId in eventsGoogleCalendarIds)
        {
            DeleteEvent(eventsGoogleCalendarId);
        }
    }

    public void AddEvent(List<string> eventAttributes)
    {
        DateTimeOffset offsetStartDateTime = DateTimeOffset.Parse(eventAttributes[3]);
        DateTimeOffset offsetEndDateTime = DateTimeOffset.Parse(eventAttributes[4]);
        DateTime convertedStartDateTime = offsetStartDateTime.DateTime;
        DateTime convertedEndDateTime = offsetEndDateTime.DateTime;
        string StartDateTime = convertedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        string EndDateTime = convertedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        
        
        _aspNetCoreNTierDbContext.Event.FromSqlRaw(
            "INSERT INTO Event(GoogleCalendarID, ModuleId, Name, Details, Starts, Ends) " +
            $"VALUES ('{eventAttributes[0]}', '{eventAttributes[5]}', '{eventAttributes[1]}', '{eventAttributes[2]}','{StartDateTime}', '{EndDateTime}'  ); ';");
    }
}

/*
 *
 *
 *1 - get calendarId from calendarIdList from current account and check if they exist in module db
2 - delete all events for current module
3 - create events


[
	[orgamizer.email,summary,description,start.dateTime,end.dateTime], //for each event
]



-----------------------------------


Create controller that returns module's events



Create controller that returns my event's
 *
 * 
 */