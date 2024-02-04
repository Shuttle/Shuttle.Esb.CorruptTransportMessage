using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.CorruptTransportMessage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorruptTransportMessage(this IServiceCollection services,
            Action<CorruptTransportMessageBuilder> builder = null)
        {
            Guard.AgainstNull(services, nameof(services));

            var corruptTransportMessageBuilder = new CorruptTransportMessageBuilder(services);

            builder?.Invoke(corruptTransportMessageBuilder);

            services.TryAddSingleton<CorruptTransportMessageHostedService, CorruptTransportMessageHostedService>();

            services.AddOptions<CorruptTransportMessageOptions>().Configure(options =>
            {
                options.MessageFolder = corruptTransportMessageBuilder.Options.MessageFolder;
            });

            services.AddPipelineModule<CorruptTransportMessageHostedService>();

            return services;
        }
    }
}