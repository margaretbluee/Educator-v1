using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories
{
    public interface IEventRepository
    {
        Event getEvent(int id);

        Event getEventByGoogleCalendarEventId(string googleCalendarEventId);

        List<Event> getEvents();

        // or another method name: getEventsByGoogleCalendarId(int moduleId);
        List<Event> getEventsByModuleId(int moduleId);

        // Returns the events that are not included in the HashSet<string>
        List<Event> GetLeftoverEvents(HashSet<string> googleCalendarEvents);

        void AddEvent(List<string> eventAttributes);

        void UpdateEvent(List<string> eventToUpdate);

        void DeleteEvent(Event eventToDelete);

        void DeleteEvents(List<Event> eventsList);

        bool ExistsEventByGoogleCalendarEventId(string GoogleCalendarEventId);

        void Save();        
    }
}
