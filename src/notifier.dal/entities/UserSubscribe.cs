using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "UserSubscribe")]
    public class UserSubscribe : BaseEntity
    {
        public string UserId { get; set; }

        public string RssId { get; set; }

        public string GroupId { get; set; }

        public bool WouldBeSentCreator { get; set; }

        public int CheckPeriod { get; set; } = 5;

        public string CronExpression { get; set; }
    }
}
