namespace notifier.dal.entities
{
    public class User : BaseEntity
    {
        public long TelegramId { get; set; }

        public string Username { get; set; }
    }
}