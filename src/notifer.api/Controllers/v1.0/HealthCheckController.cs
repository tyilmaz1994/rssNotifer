using Microsoft.AspNetCore.Mvc;
using System;

namespace notifer.api.Controllers.v1._0
{
    [Route("v1.0/healthcheck")]
    [ApiController]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return string.Concat("https://www.linkedin.com/in/tunahanyilmaz/ I'm healthy. ", DateTime.Now);
        }
    }
}