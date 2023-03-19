namespace CapOutboxSample.Processors;

public interface IMessageProcessor
{
    void SetProcessed(string processKey);
    
    bool HasProcessed(string processKey);
}