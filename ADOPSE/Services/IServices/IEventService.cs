using ADOPSE.Models;

namespace ADOPSE.Services.IServices;

public interface IEventService
{
    public void DeleteAllEvents(HashSet<string> eventsGoogleCalendarIds);

    public void AddEvent(List<string> eventAttributes);

    public IEnumerable<Event> GetEventsByStudentId(int studentId);

    public IEnumerable<Event> GetEventsByModuleId(int id);
}