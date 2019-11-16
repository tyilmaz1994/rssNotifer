namespace notifier.dal.entities
{
    public class NotiferLog : BaseEntity
    {
        public short LogLevel { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
