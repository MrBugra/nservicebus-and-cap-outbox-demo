namespace CapOutboxSample.Events;

public class WalletCreated
{
    public string Username { get; set; }

    public double Balance { get; set; }
}