using MongoDB.Bson.Serialization.Attributes;
using notifier.dal.attributes;
using System;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "UserSubscribe")]
    public class UserSubscribe : BaseEntity
    {
        public string UserId { get; set; }

        public string RssId { get; set; }

        public string GroupId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CheckDate { get; set; } = DateTime.Now;

        public bool WouldBeSentCreator { get; set; }

        public int CheckPeriod { get; set; } = 5;

        public string CronExpression { get; set; }
    }
}
