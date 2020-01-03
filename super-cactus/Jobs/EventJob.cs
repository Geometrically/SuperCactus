using System.Threading.Tasks;
using Quartz;

namespace super_cactus.Jobs
{
    public class EventJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            
        }
    }
}