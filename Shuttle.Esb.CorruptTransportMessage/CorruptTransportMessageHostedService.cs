using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.CorruptTransportMessage;

public class CorruptTransportMessageHostedService : IHostedService
{
    private readonly string _corruptTransportMessageFolder;
    private readonly IDeserializeTransportMessageObserver _deserializeTransportMessageObserver;

    public CorruptTransportMessageHostedService(IOptions<CorruptTransportMessageOptions> corruptTransportMessageOptions, IDeserializeTransportMessageObserver deserializeTransportMessageObserver)
    {
        Guard.AgainstNull(Guard.AgainstNull(corruptTransportMessageOptions).Value);

        _deserializeTransportMessageObserver = Guard.AgainstNull(deserializeTransportMessageObserver);
        _corruptTransportMessageFolder = corruptTransportMessageOptions.Value.MessageFolder;

        _deserializeTransportMessageObserver.TransportMessageDeserializationException += OnTransportMessageDeserializationException;
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

    private void OnTransportMessageDeserializationException(object? sender, DeserializationExceptionEventArgs deserializationExceptionEventArgs)
    {
        var filePath = Path.Combine(_corruptTransportMessageFolder, $"{Guid.NewGuid()}.stm");

        using (Stream file = File.OpenWrite(filePath))
        using (var stream = Guard.AgainstNull(deserializationExceptionEventArgs.PipelineContext.Pipeline.State.GetReceivedMessage()).Stream.CopyAsync().GetAwaiter().GetResult())
        {
            stream.CopyTo(file);
        }
    }
}