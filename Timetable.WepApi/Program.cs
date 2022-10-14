using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Informer;
using Timetable.Application.Interfaces;
using Timetable.Persistance;
using Timetable.Persistance.Jobs;

namespace Timetable.WepApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File("TimetableWebApiLog-.txt", rollingInterval:
                    RollingInterval.Day)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                //try
                //{
                    await DbInitializer.Initialize(service);
                //}
                //catch (Exception exception)
                //{
                //    Log.Fatal(exception, "An error occurred while dbinitializer fill db");
                //}
                TimetableScheduler.Start(service);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
