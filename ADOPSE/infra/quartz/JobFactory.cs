using Quartz.Spi;
using Quartz;

namespace ADOPSE.infra.quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;

            // Resolve job instance from the ASP.NET Core service container
            var job = _serviceProvider.GetService(jobType) as IJob;
            if (job == null)
            {
                throw new SchedulerException($"Unable to instantiate job {jobType.FullName}");
            }

            return job;
        }

        public void ReturnJob(IJob job)
        {
            // No need to do anything here in most cases
        }
    }
}
