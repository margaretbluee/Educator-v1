using ADOPSE.Services.IServices;

namespace ADOPSE.Services;

public class CalendarService : ICalendarService
{
    private readonly IModuleService _moduleService;
    private readonly IEventService _eventService;

    public CalendarService(IModuleService moduleService, IEventService eventService)
    {
        _moduleService = moduleService;
        _eventService = eventService;
    }

    public void AddEvents(List<List<string>> events)
    {
        HashSet<string> googleCalendarIds = new HashSet<string>();

        events.ForEach(current_event =>
        {
            googleCalendarIds.Add(current_event[0]);
        });

        _eventService.DeleteAllEvents(googleCalendarIds);


        events.ForEach(current_event =>
        {
            string googleCalendarId = current_event[0];
            if (_moduleService.GetModuleByCalendarId(googleCalendarId) != null)
            {
                _eventService.AddEvent(current_event);
            }
        });
        throw new NotImplementedException();
    }
}