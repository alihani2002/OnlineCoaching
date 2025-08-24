using Hangfire;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.WebUI.HangfireJobs
{
    public static class RecurringJobs
    {
        public static void RegisterJobs()
        {
            // Suspend expired subscriptions daily
            RecurringJob.AddOrUpdate<CoachingPackageRequestService>(
                recurringJobId: "CheckExpiredSubscriptions",
                methodCall: service => service.CheckExpiredSubscriptions(),
                cronExpression: Cron.Daily,
                options: new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local,
                    QueueName = "default"
                });

            // Example: Another recurring job
            // RecurringJob.AddOrUpdate<SomeService>(
            //     "AnotherJob",
            //     service => service.SomeMethod(),
            //     Cron.Hourly,
            //     new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });
        }
    }
}
