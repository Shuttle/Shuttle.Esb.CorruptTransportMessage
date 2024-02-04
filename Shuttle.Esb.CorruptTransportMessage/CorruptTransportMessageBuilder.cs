using System;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.CorruptTransportMessage
{
    public class CorruptTransportMessageBuilder
    {
        private CorruptTransportMessageOptions _corruptTransportMessageOptions = new CorruptTransportMessageOptions();
        public IServiceCollection Services { get; }

        public CorruptTransportMessageBuilder(IServiceCollection services)
        {
            Guard.AgainstNull(services, nameof(services));

            Services = services;
        }

        public CorruptTransportMessageOptions Options
        {
            get => _corruptTransportMessageOptions;
            set => _corruptTransportMessageOptions = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}