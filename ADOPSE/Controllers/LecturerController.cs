using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Event = Google.Apis.Calendar.v3.Data.Event;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class LecturerController : ControllerBase
{
    private readonly ILogger<LecturerController> _logger;
    private readonly ILecturerService _lecturerService;

    public LecturerController(ILogger<LecturerController> logger, ILecturerService lecturerService)
    {
        _logger = logger;
        _lecturerService = lecturerService;
    }

    [HttpGet]
    public IEnumerable<Lecturer> Get()
    {
        return _lecturerService.GetLecturers();
    }

    [HttpGet("dokimi")]
    public async Task<Lecturer> GetLecturer()
    {
        string ApiKey = "AIzaSyChezRdUkRUJ39V-HxmBgj9qhNZCC6CEXw";
        string calendarId = "6a1646dbcd734091a136e1ba65003858676eeb98ab176c4e5eb7116b98d1c733@group.calendar.google.com";


        var service = new CalendarService(new BaseClientService.Initializer()
        {
            ApiKey = ApiKey,
            ApplicationName = "ADOPSE",
        });

        // Then, use the EventsResource.List method to get all events from the default calendar:
        EventsResource.ListRequest request = service.Events.List(calendarId);
        request.TimeMin = DateTime.Now;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 2500;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        Events events = await request.ExecuteAsync();
        IList<Event> items = events.Items;

        // Finally, you can loop through the list of events and print out some details:
        if (items != null && items.Count > 0)
        {
            foreach (var eventItem in items)
            {
                string when = eventItem.Start.DateTime.ToString();
                if (String.IsNullOrEmpty(when))
                {
                    when = eventItem.Start.Date;
                }
                _logger.LogInformation(string.Format("{0} ({1})<br/>", eventItem.Summary, when));
            }
        }
        else
        {
            _logger.LogInformation("No upcoming events found.");
        }


        // var request = service.Events.List(calendarId);
        // // request.Fields = "items";
        // var response = await request.ExecuteAsync();
        //
        // foreach (var item in response.Items)
        // {
        //     _logger.LogInformation($"Holiday : {item.Summary}, {item.Start}, {item.End}");
        // }

        return new Lecturer
        {
            Name = "aaa"
        };
    }
}