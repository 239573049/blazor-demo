using Quartz.Spi;
using Quartz;
using AntDesign;

namespace BlazorApp.Server.Jobs
{
    public class QuartzFactory
    {
        //1、声明一个调度工厂
        private ISchedulerFactory _schedulerFactory;
        public static IScheduler _scheduler = default;
        private IJobFactory _IOCjobFactory;
        public QuartzFactory(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _IOCjobFactory = jobFactory;
        }
        public async Task<string> Start()
        {
            //2、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //这里是指定容器仓库
            _scheduler.JobFactory = _IOCjobFactory;
            //3、开启调度器
            await _scheduler.Start();
            //4、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
                            .Build();
            //5、创建任务
            var jobDetail = JobBuilder.Create<ServerInfoJob>()
                            .WithIdentity(JobKey.Create("ServiceInfo", "group"))
                            .Build();
            //6、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            ServerInfoJob._triggerKey = trigger.Key;
            return await Task.FromResult("将触发器和任务器绑定到调度器中完成");
        }
    }
}
