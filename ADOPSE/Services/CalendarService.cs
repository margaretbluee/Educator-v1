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

        //[
        // In here we should make the operation not to delete all events, but firstly check which of them does not already exist
        //  and secondly to only add these events. No need to delete the already inserted events! (this operation to insert only new events)
        // NOTE: (must take in mind the events that has been configured like the 'summary' value changed) (this operation to delete already but newly configured events)
        events.ForEach(current_event =>
        {
            
            googleCalendarIds.Add(current_event[0]);
        });

        _eventService.DeleteAllEvents(googleCalendarIds);
        //]


        events.ForEach(current_event =>
        {
            string googleCalendarId = current_event[0];
            _logger.LogInformation($"current_event[0]: {current_event[0]} \t (TEST NIKOLAS) \n");
            _logger.LogInformation($"current_event[1]: {current_event[1]} \t (TEST NIKOLAS) \n");
            //_logger.LogInformation($"current_event[0]: {current_event[0]} \t (TEST NIKOLAS) \n");

            if (_moduleService.GetModuleByCalendarId(googleCalendarId) != null)
            {
                _logger.LogInformation($"Mpika mesa file -> {googleCalendarId}");
                current_event.Add($"{_moduleService.GetModuleByCalendarId(current_event[0]).Id}");
                _eventService.AddEvent(current_event);
            }
            //else
            //{
            //    //TODO when the googleCalendarId does not exist in the db, then by some patenta we will make it fill to the desired module!!
            //}
        });
    }


    //public void AddEventsNEW(List<List<string>> events)
    //{
    //    HashSet<string> googleCalendarIds = new HashSet<string>();

    //    //[
    //    // In here we should make the operation not to delete all events, but firstly check which of them does not already exist
    //    //  and secondly to only add these events. No need to delete the already inserted events! (this operation to insert only new events)
    //    // NOTE: (must take in mind the events that has been configured like the 'summary' value changed) (this operation to delete already but newly configured events)
    //    events.ForEach(current_event =>
    //    {

    //        googleCalendarIds.Add(current_event[0]);
    //    });

    //    _eventService.DeleteAllEvents(googleCalendarIds);
    //    //]


    //    events.ForEach(current_event =>
    //    {
    //        string googleCalendarId = current_event[0];
    //        _logger.LogInformation($"current_event[0]: {current_event[0]} \t (TEST NIKOLAS) \n");
    //        _logger.LogInformation($"current_event[1]: {current_event[1]} \t (TEST NIKOLAS) \n");
    //        //_logger.LogInformation($"current_event[0]: {current_event[0]} \t (TEST NIKOLAS) \n");

    //        if (_moduleService.GetModuleByCalendarId(googleCalendarId) != null)
    //        {
    //            _logger.LogInformation($"Mpika mesa file -> {googleCalendarId}");
    //            current_event.Add($"{_moduleService.GetModuleByCalendarId(current_event[0]).Id}");
    //            _eventService.AddEvent(current_event);
    //        }
    //        //else
    //        //{
    //        //    //TODO when the googleCalendarId does not exist in the db, then by some patenta we will make it fill to the desired module!!
    //        //}
    //    });
    //}
}