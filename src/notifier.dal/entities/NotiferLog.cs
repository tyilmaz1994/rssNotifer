using notifier.dal.attributes;

namespace notifier.dal.entities
{
    [MongoCollection(Name = "NotifierLog")]
    public class NotifierLog : BaseEntity
    {
        public short LogLevel { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
