using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADOPSE.Repositories
{
    public class EventRepository : IEventRepository       
    {
        
        private readonly MyDbContext _aspNetCoreNTierDbContext;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(MyDbContext aspNetCoreNTierDbContext, ILogger<EventRepository> logger)
        {
            _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
            _logger = logger;
        }

        public Event getEvent(int id)
        {
            return _aspNetCoreNTierDbContext.Event.Where(e => e.Id == id).Single();
        }

        public Event getEventByGoogleCalendarEventId(string googleCalendarEventId)
        {
            return _aspNetCoreNTierDbContext.Event.Where(e => e.GoogleCalendarID.Equals(googleCalendarEventId)).Single();
        }

        public List<Event> getEvents()
        {
            return _aspNetCoreNTierDbContext.Event.ToList();
        }

        public List<Event> getEventsByModuleId(int moduleId)
        {
            return _aspNetCoreNTierDbContext.Event.Where(e => e.ModuleId == moduleId).ToList();
        }

        public List<Event> GetLeftoverEvents(HashSet<string> googleCalendarEvents)
        {
            // In the Model Event the GoogleCalendarId has been filtered to be displayed as GoogleCalendarEventId in order to represent its cause better
            // Nevertheless in the dB the correspond column is called GoogleCalendarEventId 

            // The 3 GoogleCalendarIDs that are hardcoded will be removed in the future

            List<Event> eventsToRemove = _aspNetCoreNTierDbContext.Event.Where(e => !googleCalendarEvents.Contains(e.GoogleCalendarID) &&
                                    e.GoogleCalendarID != "fa6a75f135fd26a0cecc4b6503add5c36d777d96d3105c48ed129156ab2d65f2@group.calendar.google.com" &&
                                    e.GoogleCalendarID != "351c3e78780a00a778ee20b09b202e69f1c7d03f8f88795e12164382d87b5d5c@group.calendar.google.com" &&
                                    e.GoogleCalendarID != "961cc12df23a15bc08cac5b487a457a64e922cfa5147f235de466881af36003d@group.calendar.google.com").ToList();

            return eventsToRemove;
        }

        public void AddEvent(List<string> eventAttributes)
        {
            DateTimeOffset offsetStartDateTime = DateTimeOffset.Parse(eventAttributes[3]);
            DateTimeOffset offsetEndDateTime = DateTimeOffset.Parse(eventAttributes[4]);
            DateTimeOffset offsetLastModificationTime = DateTimeOffset.Parse(eventAttributes[5]);
            DateTime convertedStartDateTime = offsetStartDateTime.DateTime;
            DateTime convertedEndDateTime = offsetEndDateTime.DateTime;
            DateTime convertedLastModificationTime = offsetLastModificationTime.DateTime;
            string StartDateTime = convertedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            string EndDateTime = convertedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            string LastModificationTime = convertedLastModificationTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");

            _logger.LogInformation($"INSERT INTO Event(GoogleCalendarEventID, ModuleId, Name, Details, Starts, Ends, LastModification) VALUES ('{eventAttributes[6]}', {eventAttributes[7]}, '{eventAttributes[1]}', '{eventAttributes[2]}','{StartDateTime}', '{EndDateTime}', '{LastModificationTime}'  );");

            _aspNetCoreNTierDbContext.Event.Add(new Event
            {
                Details = eventAttributes[2],
                Ends = DateTime.Parse(EndDateTime),
                GoogleCalendarID = eventAttributes[6], // GoogleCalendarEventId
                Starts = DateTime.Parse(StartDateTime),
                Name = eventAttributes[1],
                LastModification = DateTime.Parse(LastModificationTime),
                Module = _aspNetCoreNTierDbContext.Module.Where(x => x.Id == Int16.Parse(eventAttributes[7])).First()
            }
            );

            /*_aspNetCoreNTierDbContext.Event.FromSql(
                $"INSERT INTO Event(GoogleCalendarEventID, ModuleId, Name, Details, Starts, Ends) VALUES ('{eventAttributes[6]}', {eventAttributes[5]}, '{eventAttributes[1]}', '{eventAttributes[2]}','{StartDateTime}', '{EndDateTime}');");*/

            Save();
        }

        public void UpdateEvent(List<string> eventAttributes)
        {
            DateTimeOffset offsetStartDateTime = DateTimeOffset.Parse(eventAttributes[3]);
            DateTimeOffset offsetEndDateTime = DateTimeOffset.Parse(eventAttributes[4]);
            DateTimeOffset offsetLastModificationTime = DateTimeOffset.Parse(eventAttributes[5]);
            DateTime convertedStartDateTime = offsetStartDateTime.DateTime;
            DateTime convertedEndDateTime = offsetEndDateTime.DateTime;
            DateTime convertedLastModificationTime = offsetLastModificationTime.DateTime;
            string StartDateTime = convertedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            string EndDateTime = convertedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            string LastModificationTime = convertedLastModificationTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");


            Event currently_stored_event = getEventByGoogleCalendarEventId(eventAttributes[6]);

            // GoogleCalendarEventId should not be changed
            currently_stored_event.Name = eventAttributes[1];
            currently_stored_event.Details = eventAttributes[2];
            currently_stored_event.Starts = DateTime.Parse(StartDateTime);
            currently_stored_event.Ends = DateTime.Parse(EndDateTime);
            currently_stored_event.LastModification = DateTime.Parse(LastModificationTime);
            currently_stored_event.Module = _aspNetCoreNTierDbContext.Module.Where(x => x.Id == Int16.Parse(eventAttributes[7])).First();

            _logger.LogInformation($"UPDATE EVENT SET ModuleId '{eventAttributes[7]}', Name = '{eventAttributes[1]}', Details = '{eventAttributes[2]}, Starts = '{StartDateTime}', Ends = '{EndDateTime}', LastModification = '{LastModificationTime}' WHERE GoogleCalendarEventID = '{eventAttributes[6]}'");

            _aspNetCoreNTierDbContext.Update<Event>(currently_stored_event);
            Save();
        }

        public void DeleteEvent(Event eventToDelete)
        {
            _aspNetCoreNTierDbContext.Remove<Event>(eventToDelete);

            Save();
        }

        public void DeleteEvents(List<Event> eventsList)
        {
            eventsList.ForEach(eventToRemove =>
            {
                _aspNetCoreNTierDbContext.Remove<Event>(eventToRemove);
            });

            Save();
        }

        public bool ExistsEventByGoogleCalendarEventId(string googleCalendarEventId)
        {
            return _aspNetCoreNTierDbContext.Event.Any<Event>(e => e.GoogleCalendarID.Equals(googleCalendarEventId));
        }

        public void Save()
        {
            _aspNetCoreNTierDbContext.SaveChanges();
        }
    }
}
