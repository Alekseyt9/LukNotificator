
using LukNotificator.Jobs;
using Quartz;

namespace LukNotificator.Services
{
    internal class NotificationTaskManager(ISchedulerFactory schedulerFactory) : INotificationTaskManager
    {
        public async Task Start()
        {
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<NotificationJob>()
                //.WithIdentity(user.Id.ToString(), s_NotificationsGroup)
                //.UsingJobData("userId", user.Id)
                .Build();

            var trigger = TriggerBuilder.Create()
                //.WithIdentity(user.Id.ToString(), s_NotificationsGroup)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

    }
}
