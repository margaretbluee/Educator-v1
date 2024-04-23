using ADOPSE.infra.quartz;
using Quartz;

namespace ADOPSE.Services.IServices
{
    public interface IQuartzHostedService
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);

        // IJobDetail CreateJob(JobSchedule schedule);

        // ITrigger CreateTrigger(JobSchedule schedule);

    }
}
