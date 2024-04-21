using ADOPSE.Services.IServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel;
using Calendar = Google.Apis.Calendar.v3.Data.Calendar;
using GoogleApisV3CalendarService = Google.Apis.Calendar.v3.CalendarService;


namespace ADOPSE.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ILogger _logger;
        public string client_email { get; set; }
        public string private_key { get; set; }


        public Calendar RetrieveCalendarById(string calendarId)
        {
            throw new NotImplementedException();
        }

        // GET ALL CALENDARS
        public List<CalendarListEntry> RetrieveCalendars()
        {
            var service = GetCalendarService();
            var requestCalendarList = service.CalendarList.List().Execute();

            List<CalendarListEntry> calendars = (List<CalendarListEntry>)requestCalendarList.Items;
            
            foreach (var calendar in calendars)
            {                
                Console.WriteLine($"Calendar '{calendar.Summary}', DescriptionText: {calendar.Description}, CalendarId: '{calendar.Id}'");                
            }

            return calendars;
        }

        public HashSet<string> RetrieveCalendarsIds()
        {
            var service = GetCalendarService();
            var requestCalendarList = service.CalendarList.List().Execute();

            List<CalendarListEntry> calendars = (List<CalendarListEntry>)requestCalendarList.Items;
            HashSet<string> calendarIds = new HashSet<string>();

            foreach (var calendar in calendars)
            {
                calendarIds.Add(calendar.Id);
                // Console.WriteLine($"Calendar '{calendar.Summary}', DescriptionText: {calendar.Description}, CalendarId: '{calendar.Id}'");
            }

            return calendarIds;
        }


        public List<List<string>> RetrieveAllEventsFromGoogleApi()
        {
            var service = GetCalendarService();
            var requestCalendarList = service.CalendarList.List();           

            List < List<string> > events = new List<List<string>>();

            List<CalendarListEntry> calendars = (List<CalendarListEntry>)requestCalendarList.Execute().Items;

            foreach (var calendar in calendars)
            {
                Console.WriteLine();
                Console.WriteLine($"Calendar '{calendar.Summary}', DescriptionText: {calendar.Description}, CalendarId: '{calendar.Id}'");
                //Console.WriteLine("Events::");

                var eventList= RetrieveEventsListByCalendarId(calendar.Id, service);

                if (eventList != null && eventList.Count > 0)
                {
                    foreach (var evt in eventList)
                    {
                        string startWhen,  endWhen , summary, description;
                        _ = String.IsNullOrEmpty(evt.Start.DateTime.ToString()) ? startWhen = evt.Start.Date : startWhen = evt.Start.DateTime.ToString();
                        _ = String.IsNullOrEmpty(evt.End.DateTime.ToString()) ? endWhen = evt.End.Date : endWhen = evt.End.DateTime.ToString();
                        _ = String.IsNullOrEmpty(evt.Summary) ? summary = "" : summary = evt.Summary.ToString();
                        _ = String.IsNullOrEmpty(evt.Description) ? description = "" : description = evt.Description.ToString();

                        List<string> eventDetails = new List<string>
                        {
                            evt.Organizer.Email.ToString(),                            
                            summary,
                            description,
                            startWhen,
                            endWhen,
                            evt.Updated.ToString(),
                            evt.Id,
                            //evt.Status.ToString(),
                        };                        
                        Console.WriteLine($"EventId: '{evt.Id}',\n " +
                                          $"Inside Calendar with id: '{evt.Organizer.Email}',\n " +
                                          $"Summary: '{summary}'," +
                                          $" DescriptionText: '{description}'," +
                                          $" StartDate: '{startWhen}'," +
                                          $" EndDate: '{endWhen}," +
                                          $" LastUpdate: '{evt.Updated}'");
                        events.Add(eventDetails);
                    };
                }                                
            }

            return events;
        }

        // Get events list by calendarId /  GoogleApisV3CalendarService is the googleCalendarService that has been passed through
        public List<Event> RetrieveEventsListByCalendarId(string calendarId, GoogleApisV3CalendarService service)
        {
            //var service = GetCalendarService();
            var requestEvents = service.Events.List(calendarId);

            List<Event> eventsList = (List<Event>)requestEvents.Execute().Items;
            
            Console.WriteLine($"Events found: {eventsList.Count}");
            //Console.WriteLine($"For CalendarId: {calendarId} there are {eventsList.Count} events");

            var count = 0;
            foreach (var current_event in eventsList)
            {                

                Console.WriteLine($"{count}: {current_event.Id}");
                count++;
            }

            return eventsList;
        }

        // Google Calendar API Authentication and Calendar Service for the given service account
        private GoogleApisV3CalendarService GetCalendarService()
        { 
            // Fill the filePath with yours path and name of service account credential
            const string serviceAccountCredentialFilePath = ".\\serviceAccountCredential.json";
            const string regularGoogleAccount = "adopse2024@gmail.com";

            if (string.IsNullOrEmpty(serviceAccountCredentialFilePath))
                throw new Exception("Path to the service account credentials file is required.");           
            
            var jsonData = System.IO.File.ReadAllText(serviceAccountCredentialFilePath);
                                                                                             //
            CalendarService serviceAccount = JsonConvert.DeserializeObject<CalendarService>(jsonData);

                //var credential = ServiceAccountCredential.FromServiceAccountData(new FileStream("service.json", FileMode.Open));

                // Google CAlendar API Service Account Authentication via json file
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccount.client_email)
                {
                    Scopes = new[]
                    {
                        GoogleApisV3CalendarService.Scope.Calendar,
                        GoogleApisV3CalendarService.Scope.CalendarEvents,
                        GoogleApisV3CalendarService.Scope.CalendarEventsReadonly
                    },
                    
                }.FromPrivateKey(serviceAccount.private_key));


                //var token = credential.GetAccessTokenForRequestAsync().Result;

                // Create the Calendar service using the provided key file
                var service = new GoogleApisV3CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Educator using Google Calendar API"
                });

                return service;
            }

        public string CreateCalendar(string summary, string descr)
        {
            var service = GetCalendarService();
            const string regularGoogleAccount = "adopse2024@gmail.com";

            var newCalendar = new Calendar
            {
                Summary = summary,
                Description = descr
            };

            Calendar newCalendarResult = service.Calendars.Insert(newCalendar).Execute();

            AclRule rule = new AclRule
            {
                Scope = new AclRule.ScopeData
                {
                    Type = "user",
                    Value = regularGoogleAccount,
                },
                Role = "owner"
            };

            service.Acl.Insert(rule, newCalendarResult.Id).Execute();

            //_logger.LogInformation($"New calendar Created! \n" +
            //                       $"CalendarId: '{newCalendarResult.Id}' \n" +
            //                       $"Summary: '{newCalendarResult.Summary}' \n " +
            //                       $"Description: '{newCalendarResult.Description}' ");

            Console.WriteLine($"New calendar Created! \n" +
                                   $"CalendarId: '{newCalendarResult.Id}' \n" +
                                   $"Summary: '{newCalendarResult.Summary}' \n " +
                                   $"Description: '{newCalendarResult.Description}' ");

            return newCalendarResult.Id;
        }
    }
}