using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using SampleApp.Events;
using Serilog;
using Serilog.Formatting.Compact;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/log-{Date}.txt")
                .WriteTo.Console();

            Log.Logger = loggerConfig.CreateLogger();

            var services = new ServiceCollection();
            var loggerFactory = new LoggerFactory()
                .AddSerilog();

            services
                .AddSingleton(loggerFactory)
                .AddLogging()
                .AddRawRabbitBus()
                .AddOptions();

            var sp = services.BuildServiceProvider();

            RunAsync(sp)
                .GetAwaiter()
                .GetResult();
        }

        public static async Task RunAsync(IServiceProvider sp)
        {
            var bus = sp.GetRequiredService<IBusClient>();
            var logger = sp.GetRequiredService<ILogger<Program>>();

            await bus.SubscribeAsync<TestBusEvent, BusContext>(async (msg, ctx) =>
            {
                logger.LogInformation("TestBusEvent received");

                return await Task.FromResult(new Ack());
            });

            await Task.Delay(5000);

            await bus.PublishAsync(new TestBusEvent());
        }
    }
}
