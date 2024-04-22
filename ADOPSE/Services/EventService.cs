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
        List<Event> eventsToDelete = _aspNetCoreNTierDbContext.Event.Where(x => x.GoogleCalendarID == eventGoogleCalendarId).ToList();
        eventsToDelete.ForEach(eventToDelete =>
        {
            _aspNetCoreNTierDbContext.Remove<Event>(eventToDelete);
        });

        _aspNetCoreNTierDbContext.SaveChanges();
        /*_aspNetCoreNTierDbContext.Event.FromSqlRaw(
            $"delete from Event where GoogleCalendarID = '{eventGoogleCalendarId}';");*/
    }

    public void DeleteAllEvents(HashSet<string> eventsGoogleCalendarIds)
    {
        foreach (var eventsGoogleCalendarId in eventsGoogleCalendarIds)
        {
            DeleteEvent(eventsGoogleCalendarId);
        }
    }

    public IEnumerable<Event> GetEventsByStudentId(int UserId)
    {
        List<Enrolled> enrolled =
            _aspNetCoreNTierDbContext.Enrolled.Where(x => x.USERS.Id == UserId).Include(y => y.Module).ToList();

        List<int> enrolledModuleIds = new List<int>();

        enrolled.ForEach(x =>
        {
            _logger.LogInformation(x.ToString());
            enrolledModuleIds.Add(x.Module.Id);
        });

        return _aspNetCoreNTierDbContext.Event.Where(x => enrolledModuleIds.Contains(x.Module.Id));
    }

    public void AddEvent(List<string> eventAttributes)
    {
        DateTimeOffset offsetStartDateTime = DateTimeOffset.Parse(eventAttributes[3]);
        DateTimeOffset offsetEndDateTime = DateTimeOffset.Parse(eventAttributes[4]);
        DateTime convertedStartDateTime = offsetStartDateTime.DateTime;
        DateTime convertedEndDateTime = offsetEndDateTime.DateTime;
        string StartDateTime = convertedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        string EndDateTime = convertedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");

        _logger.LogInformation($"INSERT INTO Event(GoogleCalendarID, ModuleId, Name, Details, Starts, Ends) VALUES ('{eventAttributes[0]}', {eventAttributes[5]}, '{eventAttributes[1]}', '{eventAttributes[2]}','{StartDateTime}', '{EndDateTime}'  );");


        _aspNetCoreNTierDbContext.Event.Add(new Event
        {
            Details = eventAttributes[2],
            Ends = DateTime.Parse(EndDateTime),
            GoogleCalendarID = eventAttributes[0],
            Starts = DateTime.Parse(StartDateTime),
            Name = eventAttributes[1],
            Module = _aspNetCoreNTierDbContext.Module.Where(x => x.Id == Int16.Parse(eventAttributes[5])).First()
        }
        );

        /*_aspNetCoreNTierDbContext.Event.FromSql(
            $"INSERT INTO Event(GoogleCalendarID, ModuleId, Name, Details, Starts, Ends) VALUES ('{eventAttributes[0]}', {eventAttributes[5]}, '{eventAttributes[1]}', '{eventAttributes[2]}','{StartDateTime}', '{EndDateTime}');");*/

        _aspNetCoreNTierDbContext.SaveChanges();
    }
    public IEnumerable<Event> GetEventsByModuleId(int id)
    {
        return _aspNetCoreNTierDbContext.Event.Where(x => x.ModuleId == id).ToList();
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