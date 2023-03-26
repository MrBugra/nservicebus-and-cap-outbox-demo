using CapOutboxSample.Constants;
using CapOutboxSample.Events;
using CapOutboxSample.Processors;
using DotNetCore.CAP;

namespace CapOutboxSample.Subscribers;

public class WalletCreatedSubscriberV2 : ICapSubscribe
{
    private readonly IMessageProcessor _messageProcessor;

    public WalletCreatedSubscriberV2(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    [CapSubscribe(EventConstants.WalletCreated, Group = "group2")]
    public void Consumer(WalletCreated eventData, [FromCap] CapHeader header)
    {
        if (_messageProcessor.HasProcessed(header["cap-msg-id"]))
            return;
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(eventData));
    }
}