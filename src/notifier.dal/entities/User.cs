using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "User")]
    public class User : BaseEntity
    {
        public int TelegramId { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}