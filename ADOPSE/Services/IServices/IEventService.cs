namespace ADOPSE.Services.IServices;

public interface IEventService
{
    public void DeleteAllEvents(HashSet<string> eventsGoogleCalendarIds);

    public void AddEvent(List<string> eventAttributes);
}