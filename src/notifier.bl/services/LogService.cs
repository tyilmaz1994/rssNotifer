using notifier.bl.enums;
using notifier.dal.entities;
using notifier.dal.persistence;
using System;

namespace notifier.bl.services
{
    public class LogService : AbstractService<NotiferLog>, ILogService
    {
        private readonly IRepo<NotiferLog> _repo;

        public LogService(IRepo<NotiferLog> repo) : base(repo)
        {
            _repo = repo;
        }

        public void InsertLog(Exception ex, LogLevel logLevel)
        {
            _repo.Add(new NotiferLog
            {
                Active = (short)Active.Yes,
                LogLevel = (short)logLevel,
                Message = ex.Message,
                StackTrace = ex.ToString(),
            });  
        }
    }

    public interface ILogService : IAbstractService<NotiferLog>
    {
        void InsertLog(Exception ex, LogLevel logLevel);
    }
}
