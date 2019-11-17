using notifier.bl.enums;
using notifier.bl.services;
using notifier.dal.entities;
using System;
using Xunit;

namespace notifier.tests.services
{
    public class LogServiceTest : BaseServiceTest<ILogService, NotiferLog>
    {
        [Fact]
        public void InsertLog_Test()
        {
            DateTime date = DateTime.Now;

            Service.InsertLog(new Exception($"{date.ToString()}__test err"), LogLevel.ERROR);

            var log = Service.Get(x => x.Message == $"{date.ToString()}__test err");

            Assert.NotNull(log);
        }
    }
}
