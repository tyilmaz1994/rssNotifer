using Quartz;
using System;
using System.Threading.Tasks;

namespace notifier.bl.hostedServices.jobs
{
    /// <summary>
    /// Gets RSS news over link and check that if any news is added to rss then notify user
    /// </summary>
    public class RssNewsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now.ToString());
        }
    }
}