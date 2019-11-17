using notifier.bl.enums;
using notifier.dal.entities;
using notifier.dal.persistence;
using System;

namespace notifier.bl.services
{
    public class LogService : AbstractService<NotifierLog>, ILogService
    {
        private readonly IRepo<NotifierLog> _repo;

        public LogService(IRepo<NotifierLog> repo) : base(repo)
        {
            _repo = repo;
        }

        public void InsertLog(Exception ex, LogLevel logLevel)
        {
            _repo.Add(new NotifierLog
            {
                LogLevel = (short)logLevel,
                Message = ex.Message,
                StackTrace = ex.ToString(),
            });  
        }
    }

    public interface ILogService : IAbstractService<NotifierLog>
    {
        void InsertLog(Exception ex, LogLevel logLevel);
    }
}
