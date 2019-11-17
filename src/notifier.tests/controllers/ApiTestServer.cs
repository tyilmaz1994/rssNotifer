using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using notifer;
using System.Net.Http;

namespace notifier.tests.controllers
{
    public class ApiTestServer
    {
        protected readonly HttpClient Client;
        protected readonly TestServer TestServer;

        protected ApiTestServer()
        {
            TestServer = CreateServer();
            Client = CreateHttpClient();
        }

        protected TestServer CreateServer()
        {
            return new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        protected HttpClient CreateHttpClient()
        {
            return TestServer.CreateClient();
        }

        protected HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }

        protected string GetStringFromResponse(HttpResponseMessage httpResponse)
        {
            return httpResponse.Content.ReadAsStringAsync().Result;
        }

        protected T GetService<T>()
        {
            return (T)TestServer.Services.GetService(typeof(T));
        }
    }
}
