using System;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.CorruptTransportMessage;

public class CorruptTransportMessageBuilder
{
    private CorruptTransportMessageOptions _corruptTransportMessageOptions = new();

    public CorruptTransportMessageBuilder(IServiceCollection services)
    {
        Services = Guard.AgainstNull(services);
    }

    public CorruptTransportMessageOptions Options
    {
        get => _corruptTransportMessageOptions;
        set => _corruptTransportMessageOptions = Guard.AgainstNull(value);
    }

    public IServiceCollection Services { get; }
}