namespace NServiceBusOutboxSample.Models;

public class PostWalletRequest
{
    public string Username { get; set; }

    public double Balance { get; set; }
}