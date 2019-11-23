using Microsoft.Extensions.Hosting;
using notifier.bl.enums;
using notifier.bl.helpers;
using notifier.bl.services;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace notifier.bl.hostedServices
{
    public class ScheduleHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;
        private readonly IUserSubscribeService _userSubscribeService;

        public ScheduleHostedService(IScheduler scheduler, IUserSubscribeService userSubscribeService)
        {
            _scheduler = scheduler;
            _userSubscribeService = userSubscribeService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleTriggerAndJobs();
            return Task.CompletedTask;
        }

        private void ScheduleTriggerAndJobs()
        {
            var rssJob = ScheduleHelper.CreateRSSJob();
            _scheduler.AddJob(rssJob, true).GetAwaiter();
            CreateTriggersEveryXMin(rssJob);
            CreateTriggersCronExp(rssJob);
        }

        private void CreateTriggersEveryXMin(IJobDetail job)
        {
            var subscriptions = _userSubscribeService.GetList(x => x.Active == (short)Active.Yes && x.CronExpression == null);

            foreach (var item in subscriptions)
                _scheduler.ScheduleJob(ScheduleHelper.CreateTriggerEveryMin(item, job));
        }

        private void CreateTriggersCronExp(IJobDetail job)
        {
            var subscriptions = _userSubscribeService.GetList(x => x.Active == (short)Active.Yes && x.CronExpression != null);

            foreach (var item in subscriptions)
                _scheduler.ScheduleJob(ScheduleHelper.CreateTriggerCronExpression(item, job));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scheduler.Shutdown();
            return Task.CompletedTask;
        }
    }
}