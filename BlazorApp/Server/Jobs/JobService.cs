using Quartz.Impl;
using Quartz.Spi;
using Quartz;

namespace BlazorApp.Server.Jobs
{
    public static class JobService
    {
        public static QuartzFactory? _quartzFactory;
        public static IServiceCollection AddJobServiceStep(this IServiceCollection services)
        {
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzFactory>();
            services.AddSingleton<ServerInfoJob>();
            services.AddSingleton<IJobFactory, IOCJobFactory>();
            return services;
        }
        public static WebApplication UseJobServiceStep(this WebApplication app)
        {
            _quartzFactory = app.Services.GetRequiredService<QuartzFactory>();
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                await _quartzFactory.Start();
                await QuartzFactory._scheduler!.PauseTrigger(ServerInfoJob._triggerKey!);
            });
            return app;
        }
    }
}
