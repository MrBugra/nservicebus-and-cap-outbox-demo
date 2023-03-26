using CapOutboxSample.Processors;
using NServiceBus.Pipeline;

namespace NServiceBusOutboxSample.Behaviors;

public class NServiceBusMessageProcessorPipelineBehavior : Behavior<IIncomingLogicalMessageContext>
{
    private readonly IMessageProcessor _messageProcessor;

    public NServiceBusMessageProcessorPipelineBehavior(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var messageId = context.MessageId;
        Console.WriteLine($"incoming message => {Newtonsoft.Json.JsonConvert.SerializeObject(context.Message.Instance)}");

        await next().ConfigureAwait(false);
        
        _messageProcessor.SetProcessed(messageId);
        
        Console.WriteLine($"message processed => {Newtonsoft.Json.JsonConvert.SerializeObject(context.Message.Instance)}");
    }
}