using notifer.api.Controllers.v1._0;
using notifer.api.models;
using notifier.bl.services;
using notifier.dal.entities;

namespace notifier.tests.controllers
{
    public class LogControllerTest : BaseControllerTest<LogController, ILogService, BaseRequestModel<NotiferLog>, BaseResponseModel<NotiferLog>, NotiferLog>
    {
        
    }
}