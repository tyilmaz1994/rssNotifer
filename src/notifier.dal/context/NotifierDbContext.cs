namespace notifier.dal.context
{
    public class NotifierDbContext : INotifierDbContext
    {
        public string ConnectionString { get; set; }

        public string LogCollectionName { get; set; }

        public string DatabaseName { get; set; }
    }

    public interface INotifierDbContext
    {
        string ConnectionString { get; set; }

        string LogCollectionName { get; set; }

        string DatabaseName { get; set; }
    }
}