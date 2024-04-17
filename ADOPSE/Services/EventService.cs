using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Services;

public class EventService : IEventService
{
    private readonly MyDbContext _aspNetCoreNTierDbContext;
    private readonly IModuleService _moduleService;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<EventService> _logger;


    public EventService(MyDbContext aspNetCoreNTierDbContext, IModuleService moduleService, ILogger<EventService> logger, IEventRepository eventRepository)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
        _moduleService = moduleService;
        _eventRepository = eventRepository;
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

    // Parameter 0: All the retrieved events from GooglecalendarAPI
    // Processing : Deletes unsupported events on dB - Refresh updated events on dB - Add new events on dB
    public void Sync(List<List<string>> events)
    {
        List<List<string>> filteredEventsToAdd = new List<List<string>>();
        
        List<List<string>> eventsToAdd = DeleteCancelledEvents(events);


        eventsToAdd.ForEach(current_event =>
        {
            string googleCalendarId = current_event[0];
            if (_moduleService.GetModuleByCalendarId(googleCalendarId) != null)
            {
                current_event.Add($"{_moduleService.GetModuleByCalendarId(current_event[0]).Id}");
                filteredEventsToAdd.Add(current_event);
            }
        });
                  
        filteredEventsToAdd = AddEvents(eventsToAdd);
    }

    // Summary    : Removes the events that have been deleted in Google Calendar, therefore they must be deleted from our dB too
    // Parameter 0: All the retrieved events from GooglecalendarAPI
    // Processing : Deletes the events from dB that are currently in it, but do not anymore exist in the GoogleEvents List 
    // Returns    : All the events from GoogleCalendarAPI which are new or already stored in the dB but modified/unmodified
    public List<List<string>> DeleteCancelledEvents(List<List<string>> googleCalendarEvents)
    {
        HashSet<string> existingEventsList = new HashSet<string>();
        List<List<string>> eventsToAdd = new List<List<string>>();

        foreach (var googleCalendarEvent in googleCalendarEvents)
        {
            //existingEventsList.Add(googleCalendarEvent[0]); // contains the first parameter of the event parameter list which is the googleCalendarId
            existingEventsList.Add(googleCalendarEvent[6]); // the 6th parameter of the event parameter list which is the googleCalendarEventId
            eventsToAdd.Add(googleCalendarEvent);
        }

        List<Event> eventsToRemove = _eventRepository.GetLeftoverEvents(existingEventsList);

        _eventRepository.DeleteEvents(eventsToRemove);        

        return eventsToAdd;
    }

    // Parameter : All the events from GoogleCalendarAPI which are new or already stored in the dB but modified/unmodified
    // Processing: Adds new events on dB. Updates already existing events that have been updated after the googleCalendarAPI call
    // Returns   : All the events from GoogleCalendarAPI that are new or already stored in the dB but only newly modified  
    public List<List<string>> AddEvents(List<List<string>> googleCalendarEvents)
    {
        List<List<string>> newEventsList = new List<List<string>>();

        foreach (var googleCalendarEvent in googleCalendarEvents)
        {            

            var eventInDB = GetEventByGoogleCalendarEventId(googleCalendarEvent[6]); // eventExistsByGoogleCalendarEventID(string googleCalendarEvent)


            if (eventInDB != null && googleCalendarEvent[6] == eventInDB.GoogleCalendarID.ToString())
            {
                _logger.LogInformation($"Event with GoogleCalendarEventid: '{eventInDB.Id}' FOUND (EventService.AddEvents)");

                if (googleCalendarEvent[5] != eventInDB.LastModification.ToString())
                {
                    _eventRepository.UpdateEvent(googleCalendarEvent);
                    //_aspNetCoreNTierDbContext.Remove<Event>(eventInDB);
                    //_eventRepository.AddEvent(googleCalendarEvent);
                }
                else
                {
                    // event's already stored and up to date
                }
            }
            else
            {
                _eventRepository.AddEvent(googleCalendarEvent);
            }
        }
        return newEventsList;
    }

    public IEnumerable<Event> GetEventsByStudentId(int studentId)
    {
        List<Enrolled> enrolled =
            _aspNetCoreNTierDbContext.Enrolled.Where(x => x.Student.Id == studentId).Include(y => y.Module).ToList();

        List<int> enrolledModuleIds = new List<int>();

        enrolled.ForEach(x =>
        {
            _logger.LogInformation(x.ToString());
            enrolledModuleIds.Add(x.Module.Id);
        });

        return _aspNetCoreNTierDbContext.Event.Where(x => enrolledModuleIds.Contains(x.Module.Id));
    }

    // Adds the event on dB
    public void AddEvent(List<string> eventAttributes)
    {
        _eventRepository.AddEvent(eventAttributes);       
    }

    public IEnumerable<Event> GetEventsByModuleId(int id)
    {
        return _aspNetCoreNTierDbContext.Event.Where(x => x.ModuleId == id).ToList();
    }

    public Event GetEventByGoogleCalendarEventId(string eventId)
    {
        return _aspNetCoreNTierDbContext.Event.Include(x => x.Module).Where(x => x.GoogleCalendarID == eventId).FirstOrDefault();
    }

    //returns the event according to moduleId(including the details of module)
    public Event GetEventByModuleId(int moduleId)
    {
        return _aspNetCoreNTierDbContext.Event.Where(e => e.ModuleId == moduleId).FirstOrDefault();
    }

    // returns the googleCalendarId of event according to moduleId
    //public IQueryable<string> GetGoogleCalendarIdByModuleId(int moduleId)
    //{
    //    return _aspNetCoreNTierDbContext.Event.Where(e => e.ModuleId == moduleId).Select(e => e.Module.GoogleCalendarID);

    //}

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