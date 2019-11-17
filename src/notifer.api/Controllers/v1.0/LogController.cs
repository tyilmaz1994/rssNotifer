using Microsoft.AspNetCore.Mvc;
using notifer.api.models;
using notifier.bl.services;
using notifier.dal.entities;

namespace notifer.api.Controllers.v1._0
{
    [Route("v1.0/log")]
    [ApiController]
    public class LogController : AbstractController<BaseRequestModel<NotifierLog>, BaseResponseModel<NotifierLog>, ILogService, NotifierLog>
    {
        private readonly ILogService _logService;

        public LogController(ILogService service) : base(service)
        {
            _logService = service;
        }
    }
}