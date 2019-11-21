using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "UserRss")]
    public class UserRss : BaseEntity
    {
        public string Url { get; set; }

        public string UserId { get; set; }
    }
}