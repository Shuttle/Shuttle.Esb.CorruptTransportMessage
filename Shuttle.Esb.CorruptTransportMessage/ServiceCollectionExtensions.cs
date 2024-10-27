using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.CorruptTransportMessage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorruptTransportMessage(this IServiceCollection services, Action<CorruptTransportMessageBuilder>? builder = null)
    {
        var corruptTransportMessageBuilder = new CorruptTransportMessageBuilder(Guard.AgainstNull(services));

        builder?.Invoke(corruptTransportMessageBuilder);

        services.TryAddSingleton<CorruptTransportMessageHostedService, CorruptTransportMessageHostedService>();

        services.AddOptions<CorruptTransportMessageOptions>().Configure(options =>
        {
            options.MessageFolder = corruptTransportMessageBuilder.Options.MessageFolder;
        });

        services.AddHostedService<CorruptTransportMessageHostedService>();

        return services;
    }
}