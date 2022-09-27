using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using Timetable.Application.Interfaces;
using Timetable.Persistance;

namespace Timetable.WepApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File("TimetableWebApiLog-.txt", rollingInterval:
                    RollingInterval.Day)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();
            try
            {
                var context = host.Services.CreateScope().ServiceProvider.GetRequiredService<ITimetableDbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "An error occurred while app initialization");
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
