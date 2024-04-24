using ADOPSE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Services.IServices;

public interface IEventService
{
    public void DeleteAllEvents(HashSet<string> eventsGoogleCalendarIds);

    // Parameter 0: All the retrieved events from GooglecalendarAPI
    // Processing : Deletes unsupported events on dB - Refresh updated events on dB - Add new events on dB
    public void Sync(List<List<string>> events);

    // Parameter 0: All the retrieved events from GooglecalendarAPI
    // Processing : Deletes the events from dB that are currently in it, but do not anymore exist in the GoogleEvents List 
    // Returns    : All the events from GoogleCalendarAPI which are new or already stored in the dB but modified/unmodified
    public List<List<string>> DeleteCancelledEvents(List<List<string>> GoogleCalendarEvents);

    // Parameter : All the events from GoogleCalendarAPI which are new or already stored in the dB but modified/unmodified
    // Processing: Adds new events on dB. Updates already existing events that have been updated after the googleCalendarAPI call
    // Returns   : All the events from GoogleCalendarAPI that are new or already stored in the dB but only newly modified  
    public void AddEvents(List<List<string>> googleCalendarEvents);

    public void AddEvent(List<string> eventAttributes);

    public IEnumerable<Event> GetEventsByStudentId(int studentId);

    public IEnumerable<Event> GetEventsByModuleId(int id);

    public Event GetEventByGoogleCalendarEventId(string eventId);

    public Event GetEventByModuleId(int moduleId);

    //public IQueryable<string> GetGoogleCalendarIdByModuleId(int moduleId);
}