using System;
using System.Linq.Expressions;
using notifier.bl.helpers;
using notifier.dal.entities;
using notifier.dal.persistence;
using Quartz;

namespace notifier.bl.services
{
    public class ScheduleSubscribeService : UserSubscribeService, IScheduleSubscribeService
    {
        private readonly IScheduler _scheduler;

        public ScheduleSubscribeService(IRepo<UserSubscribe> repo, IScheduler scheduler) : base(repo)
        {
            _scheduler = scheduler;
        }

        /// <summary>
        /// Save user and also schedule subscribed model.
        /// </summary>
        public override UserSubscribe Save(UserSubscribe entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity = base.Save(entity);
                _scheduler.ScheduleJob(ScheduleHelper.CreateTriggerEveryMin(entity, ScheduleHelper.CreateRSSJob()));
            }

            return entity;
        }

        /// <summary>
        /// Delete user also unschedule deleted model.
        /// </summary>
        public override void Delete(Expression<Func<UserSubscribe, bool>> expression)
        {
            var entity = base.Get(expression);
            _scheduler.UnscheduleJob(new TriggerKey(entity.Id, entity.UserId));
            base.Delete(expression);
        }
    }

    public interface IScheduleSubscribeService : IUserSubscribeService
    {

    }
}
