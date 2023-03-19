using CapOutboxSample.Data;
using CapOutboxSample.Filters;
using CapOutboxSample.Subscribers;

namespace CapOutboxSample.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddCapServices(this IServiceCollection services)
    {
        services.AddCap(x =>
        {
            x.UseEntityFramework<WalletDbContext>();
            
            x.UseRabbitMQ(opt =>
            {
                opt.Port = 5672;
                opt.HostName = "localhost";
                opt.Password = "guest";
                opt.UserName = "guest";
                opt.VirtualHost = "/"; 
            });

            x.FailedRetryCount = 3;
            x.FailedRetryInterval = 5; // seconds
            
            x.UseDashboard(d =>
            {
                d.UseChallengeOnAuth = false;
            });
        })
            .AddSubscribeFilter<CapFilter>();

        services.AddTransient<WalletCreatedSubscriber>();
        services.AddTransient<WalletCreatedSubscriberV2>();
        services.AddTransient<WalletCreatedCallbackSubscriber>();
    } 
}