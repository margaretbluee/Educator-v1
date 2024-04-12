using Google.Apis.Calendar.v3.Data;
using GoogleApisV3CalendarService = Google.Apis.Calendar.v3.CalendarService;

namespace ADOPSE.Services.IServices
{
    public interface IGoogleCalendarService
    {
        public List<CalendarListEntry> RetrieveCalendars();
        public List<List<string>> RetrieveAllEventsFromGoogleApi();
        public List<Event> RetrieveEventsListByCalendarId(string calendarId, GoogleApisV3CalendarService calendarService);
        public Calendar RetrieveCalendarById(string calendarId);
        public string CreateCalendar(string summary, string description);
    }
}
