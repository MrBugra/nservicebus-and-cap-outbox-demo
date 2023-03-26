using CapOutboxSample.Constants;
using CapOutboxSample.Events;
using CapOutboxSample.Processors;
using DotNetCore.CAP;

namespace CapOutboxSample.Subscribers;

public class WalletCreatedCallbackSubscriber : ICapSubscribe
{
    private readonly IMessageProcessor _messageProcessor;

    public WalletCreatedCallbackSubscriber(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    [CapSubscribe(EventConstants.WalletCreatedCallback, Group = "group1")]
    public void Consumer([FromCap] CapHeader header)
    {
        if (_messageProcessor.HasProcessed(header["cap-msg-id"]))
            return;
            
        Console.WriteLine("callback-succeeded");
    }
}