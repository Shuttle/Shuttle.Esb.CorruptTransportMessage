using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.CorruptTransportMessage
{
    public class CorruptTransportMessageHostedService : IHostedService
    {
        private readonly IDeserializeTransportMessageObserver _deserializeTransportMessageObserver;
        private readonly string _corruptTransportMessageFolder;

        public CorruptTransportMessageHostedService(IOptions<CorruptTransportMessageOptions> corruptTransportMessageOptions, IDeserializeTransportMessageObserver deserializeTransportMessageObserver)
        {
            Guard.AgainstNull(corruptTransportMessageOptions, nameof(corruptTransportMessageOptions));
            Guard.AgainstNull(corruptTransportMessageOptions.Value, nameof(corruptTransportMessageOptions.Value));

            _deserializeTransportMessageObserver = Guard.AgainstNull(deserializeTransportMessageObserver, nameof(deserializeTransportMessageObserver));
	        _corruptTransportMessageFolder = corruptTransportMessageOptions.Value.MessageFolder;

            _deserializeTransportMessageObserver.TransportMessageDeserializationException += OnTransportMessageDeserializationException;
        }

        private void OnTransportMessageDeserializationException(object sender, DeserializationExceptionEventArgs deserializationExceptionEventArgs)
        {
            var filePath = Path.Combine(_corruptTransportMessageFolder, $"{Guid.NewGuid()}.stm");

            using (Stream file = File.OpenWrite(filePath))
            using (var stream = deserializationExceptionEventArgs.PipelineEvent.Pipeline.State.GetReceivedMessage().Stream.Copy())
            {
                stream.CopyTo(file);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _deserializeTransportMessageObserver.TransportMessageDeserializationException -= OnTransportMessageDeserializationException;

            await Task.CompletedTask;
        }
    }
}