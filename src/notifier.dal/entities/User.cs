using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "User")]
    public class User : BaseEntity
    {
        public long TelegramId { get; set; }

        public string Username { get; set; }
    }
}