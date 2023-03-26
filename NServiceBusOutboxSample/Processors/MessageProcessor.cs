using CapOutboxSample.Processors;
using Microsoft.Extensions.Caching.Memory;

namespace NServiceBusOutboxSample.Processors;

public class MessageProcessor : IMessageProcessor
{
    private readonly IMemoryCache _memoryCache;

    public MessageProcessor(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public void SetProcessed(string processKey)
    {
        _memoryCache.Set(processKey,true,TimeSpan.FromMinutes(5));
    }

    public bool HasProcessed(string processKey)
    {
        return _memoryCache.Get<bool>(processKey);
    }
}