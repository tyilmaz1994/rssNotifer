using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace notifer.api.factories.scheduleListeners
{
    public class JobListener : IJobListener
    {
        public string Name { get { return "job"; } }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {

            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {

            return Task.CompletedTask;
        }
    }
}
