using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Services;

public class EventService : IEventService
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;

    public EventService(MyDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
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
        throw new NotImplementedException(); //Ta start kai end theloun string refactoring
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