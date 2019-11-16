using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using notifer.api.models;
using notifier.bl.enums;
using notifier.bl.services;
using notifier.dal.entities;
using System;

namespace notifer.api.filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomExceptionFilterAttribute(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            ILogService logService = (ILogService)_serviceProvider.GetService(typeof(ILogService));
            logService.InsertLog(context.Exception, LogLevel.ERROR);

            var response = new BaseResponseModel<NotiferLog>();
            response.AddMessage(context.Exception.Message, isSuccessMessage: false);
            context.Result = new JsonResult(response);
        }
    }
}