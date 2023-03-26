using CapOutboxSample.Processors;
using NServiceBus;
using NServiceBusOutboxSample.Events;

namespace NServiceBusOutboxSample.Subscribers;

public class WalletCreatedSubscriber : IHandleMessages<WalletCreated>
{
    private readonly IMessageProcessor _messageProcessor;

    public WalletCreatedSubscriber(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    public async Task Handle(WalletCreated message, IMessageHandlerContext context)
    {
        if (_messageProcessor.HasProcessed(context.MessageId))
            return;
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(message));

        await Task.CompletedTask;
    }
}