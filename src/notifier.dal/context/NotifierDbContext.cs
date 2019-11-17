namespace notifier.dal.context
{
    public class NotifierDbContext : INotifierDbContext
    {
        public string ConnectionString { get { return "mongodb://localhost:27017"; } }

        public string LogCollectionName { get { return "log"; } }

        public string DatabaseName { get { return "dbNotify"; } } 
    }

    public interface INotifierDbContext
    {
        string ConnectionString { get; }

        string LogCollectionName { get; }

        string DatabaseName { get; }
    }
}