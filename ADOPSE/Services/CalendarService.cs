using ADOPSE.Services.IServices;

namespace ADOPSE.Services;

public class CalendarService : ICalendarService
{
    private readonly IModuleService _moduleService;
    private readonly IEventService _eventService;
    private readonly ILogger<CalendarService> _logger;

    public CalendarService(IModuleService moduleService, IEventService eventService, ILogger<CalendarService> logger)
    {
        _moduleService = moduleService;
        _eventService = eventService;
        _logger = logger;
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
                _logger.LogInformation($"Mpika mesa file -> {googleCalendarId}");
                current_event.Add($"{_moduleService.GetModuleByCalendarId(current_event[0]).Id}");
                _eventService.AddEvent(current_event);
            }
        });
    }
}