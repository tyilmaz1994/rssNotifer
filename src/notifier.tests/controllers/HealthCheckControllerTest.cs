using notifer.api;
using notifer.api.Controllers.v1._0;
using System;
using Xunit;

namespace notifier.tests.controllers
{
    public class HealthCheckControllerTest : ApiTestServer
    {
        [Fact]
        public void Get_Test_OverHttp()
        {
            var resposne = GetResponse("/v1.0/healthcheck");
            resposne.EnsureSuccessStatusCode();
            var result = GetStringFromResponse(resposne);
            Assert.StartsWith(ApiResource.healtcheck_answer, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void Get_Test()
        {
            HealthCheckController healthCheckController = new HealthCheckController();
            var result = healthCheckController.Get();
            Assert.StartsWith(ApiResource.healtcheck_answer, result.Value, StringComparison.InvariantCulture);
        }
    }
}