using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.vNext;
using RawRabbit.vNext.Pipe;

namespace SampleApp
{
    public static class RawRabbitBusExtensions
    {
        public static IServiceCollection AddRawRabbitBus(this IServiceCollection services)
        {
            return services
                .AddRawRabbit(new RawRabbitOptions
                {
                    Plugins = p => p
                        .UseContextForwarding()
                        .UseMessageContext<BusContext>()
                        .UseGlobalExecutionId()
                        .UseStateMachine()
                        .UseCustomQueueSuffix()
                });
        }
    }
}