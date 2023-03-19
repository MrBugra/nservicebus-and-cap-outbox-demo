using CapOutboxSample.Constants;
using CapOutboxSample.Models;
using CapOutboxSample.Processors;
using DotNetCore.CAP;
using Newtonsoft.Json;

namespace CapOutboxSample.Subscribers;

public class WalletCreatedSubscriber : ICapSubscribe
{
    private readonly IMessageProcessor _messageProcessor;

    public WalletCreatedSubscriber(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    [CapSubscribe(EventConstants.WalletCreated, Group = "group1")]
    public void Consumer(PostWalletRequest eventData, [FromCap] CapHeader header)
    {
        if (_messageProcessor.HasProcessed(header["cap-msg-id"]))
            return;
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(eventData));
    }
}