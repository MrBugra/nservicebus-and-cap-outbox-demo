using CapOutboxSample.Processors;
using DotNetCore.CAP.Filter;
using Newtonsoft.Json;

namespace CapOutboxSample.Filters;

public class CapFilter : SubscribeFilter
{
    private readonly IMessageProcessor _messageProcessor;

    public CapFilter(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    public override Task OnSubscribeExecutingAsync(ExecutingContext context)
    {
        Console.WriteLine(JsonConvert.SerializeObject(context.ConsumerDescriptor.TopicName));
        return base.OnSubscribeExecutingAsync(context);
    }

    public override Task OnSubscribeExecutedAsync(ExecutedContext context)
    {
        if (context.DeliverMessage.Headers.ContainsKey("cap-msg-id"))
            _messageProcessor.SetProcessed(context.DeliverMessage.Headers["cap-msg-id"]);
        Console.WriteLine($"subscribed => {JsonConvert.SerializeObject(context.ConsumerDescriptor.TopicName)}");
        return base.OnSubscribeExecutedAsync(context);
    }

    public override Task OnSubscribeExceptionAsync(ExceptionContext context)
    {
        Console.WriteLine($"exception => {JsonConvert.SerializeObject(context.ConsumerDescriptor.TopicName)}");
        return base.OnSubscribeExceptionAsync(context);
    }
}