using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace notifer.api.factories
{
    public class ScheduleJobFactory : SimpleJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScheduleJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)this._serviceProvider.GetService(bundle.JobDetail.JobType);
        }
    }
}