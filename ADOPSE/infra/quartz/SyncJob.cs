using ADOPSE.Services;
using ADOPSE.Services.IServices;
using Quartz;

namespace ADOPSE.infra.quartz
{
    [DisallowConcurrentExecution]
    public class SyncJob : IJob
    {
        private readonly ICalendarService _calendarService;
        private readonly IEventService _eventService;
        private readonly IServiceProvider _serviceProvider;

        //public SyncJob(ICalendarService calendarservice, IEventService eventService, IServiceProvider serviceProvider)
        //{
        //    _calendarService = calendarservice;
        //    _eventService = eventService;
        //    _serviceProvider = serviceProvider;
        //}

        public SyncJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var calendarService = scope.ServiceProvider.GetRequiredService<ICalendarService>();
                    var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                    var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();

                    var eventsList = calendarService.RetrieveAllEventsFromGoogleApi();
                    var calendarIds = calendarService.RetrieveCalendarsIds();

                    eventService.Sync(eventsList);
                    moduleService.SyncGoogleCalendarsIdsOfModules(calendarIds);

                    Console.WriteLine($"Job ran at {DateTime.UtcNow}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during sync execution: " + ex.Message);
            }


            return Task.CompletedTask;
        }
    }
}
