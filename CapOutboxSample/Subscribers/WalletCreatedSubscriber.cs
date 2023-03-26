using CapOutboxSample.Constants;
using CapOutboxSample.Events;
using CapOutboxSample.Processors;
using DotNetCore.CAP;

namespace CapOutboxSample.Subscribers;

public class WalletCreatedSubscriber : ICapSubscribe
{
    private readonly IMessageProcessor _messageProcessor;

    public WalletCreatedSubscriber(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    [CapSubscribe(EventConstants.WalletCreated, Group = "group1")]
    public void Consumer(WalletCreated eventData, [FromCap] CapHeader header)
    {
        if (_messageProcessor.HasProcessed(header["cap-msg-id"]))
            return;
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(eventData));
    }
}