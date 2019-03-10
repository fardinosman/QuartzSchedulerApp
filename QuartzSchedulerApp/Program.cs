using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzSchedulerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            RunProgram().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        private static async Task RunProgram()
        {
            try
            {
                
                // Grab the schedular instance from the factory
                NameValueCollection props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" }
                };

                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                
                IScheduler scheduler = await factory.GetScheduler();
                
            

                // and start it off

                await scheduler.Start();
                //define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                      .WithIdentity("job1", "group1")

                      .Build();

                //IJobDetail job = JobBuilder.Create<DumJob>()
                //            .WithIdentity("job1", "group1")
                //            .UsingJobData("jobSays", "Hello World")
                //            .UsingJobData("myFloatValue", 3.141f)
                //            .Build();


                //DateBuilder
                //// Trigger the job to run now, and then repeat every 10 seconds
                //ITrigger trigger = TriggerBuilder.Create()
                //            .WithIdentity("trigger1", "group1")
                //            .WithSimpleSchedule(x => x
                //                .WithIntervalInMinutes(5)
                //                .RepeatForever())
                //            .EndAt(DateBuilder.DateOf(22, 0, 0))

                //                .Build();
                //ITrigger crontrigger = TriggerBuilder.Create()
                //    .WithIdentity("trigger1", "group1")

                //    .WithCronSchedule("0 0/1 8-17 * * ?")

                //    .ForJob("job1", "group1")

                //    .Build();

                //Build a trigger that will fire daily at 10:42 am:
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(14, 56))
                    .ForJob("job1", "group1")
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

                // some sleep to show what is hapening
                //await Task.Delay(TimeSpan.FromSeconds(60));

                // and last shutdowen when your ready to  close your program
                // await scheduler.Shutdown();
                Console.WriteLine("Hello World where are you i am at home its very cold here i am  sitting ant trying to learn programering" +
                    "but its is very difficult iam not sure that one day i will become a programmer its exsactly sunday " +
                    "");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    public class ConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongDateString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }
    }
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Greetings  from HelloJob!");
        }
    }
    //public class DumJob : IJob
    //{
    //    public string jobSays { private get; set; }
    //    public float myFloatValue {private get; set; }
    //    public async Task Execute(IJobExecutionContext context)
    //    {
    //        JobKey key = context.JobDetail.Key;
    //        JobDataMap dataMap = context.MergedJobDataMap; // Note the difference from pervious exsample

    //        IList<DateTimeOffset> state = (IList < DateTimeOffset >)dataMap["myStateData"];
    //        state.Add(DateTimeOffset.UtcNow);


    //        await Console.Error.WriteLineAsync("Instance " + key + " of DumJob says: " + jobSays + ", and val is: " + myFloatValue);
    //    }
    //}
}
