using notifier.bl.const_;
using notifier.bl.hostedServices.jobs;
using notifier.dal.entities;
using Quartz;

namespace notifier.bl.helpers
{
    public static class ScheduleHelper
    {
        public static ITrigger CreateTriggerEveryMin(UserSubscribe userSubscribe, IJobDetail job)
        {
            return TriggerBuilder.Create()
                .ForJob(job)
                .UsingJobData(ScheduleConsts.USER_ID, userSubscribe.UserId)
                .UsingJobData(ScheduleConsts.RSS_ID, userSubscribe.RssId)
                .UsingJobData(ScheduleConsts.GROUP_ID, userSubscribe.GroupId)
                .UsingJobData(ScheduleConsts.CHECKDATE, userSubscribe.CheckDate.ToString())
                .WithIdentity(userSubscribe.Id, userSubscribe.UserId)
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();
        }

        public static ITrigger CreateTriggerCronExpression(UserSubscribe userSubscribe, IJobDetail job)
        {
            return TriggerBuilder.Create()
                .ForJob(job)
                .UsingJobData(ScheduleConsts.USER_ID, userSubscribe.UserId)
                .UsingJobData(ScheduleConsts.RSS_ID, userSubscribe.RssId)
                .UsingJobData(ScheduleConsts.GROUP_ID, userSubscribe.GroupId)
                .UsingJobData(ScheduleConsts.CHECKDATE, userSubscribe.CheckDate.ToString())
                .WithIdentity(userSubscribe.Id, userSubscribe.UserId)
                .StartNow()
                .WithCronSchedule(userSubscribe.CronExpression)
                .Build();
        }

        public static IJobDetail CreateRSSJob()
        {
            return JobBuilder.Create<RssNewsJob>()
                .WithIdentity(typeof(RssNewsJob).FullName)
                .StoreDurably()
                .Build();
        }
    }
}