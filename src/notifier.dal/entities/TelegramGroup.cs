using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "TelegramGroup")]
    public class TelegramGroup : BaseEntity
    {
        public string Username { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public long ChatId { get; set; }

        public string UserId { get; set; }
    }
}
