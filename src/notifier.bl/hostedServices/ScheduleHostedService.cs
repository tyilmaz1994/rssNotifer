using Microsoft.Extensions.Hosting;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace notifier.bl.hostedServices
{
    public class ScheduleHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public ScheduleHostedService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scheduler.Shutdown();
            return Task.CompletedTask;
        }
    }
}
