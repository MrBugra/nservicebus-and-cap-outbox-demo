using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.TransactionalSession;
using NServiceBusOutboxSample.Behaviors;
using NServiceBusOutboxSample.Data;

namespace NServiceBusOutboxSample.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddNServiceBusHost(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Wallet.Sender");
                var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                transport.UseConventionalRoutingTopology(QueueType.Quorum);
                transport.ConnectionString("host=localhost;username=guest;password=guest");
                transport.Transactions(TransportTransactionMode.ReceiveOnly);
                endpointConfiguration.EnableInstallers();

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
                dialect.JsonBParameterModifier(
                    modifier: parameter =>
                    {
                        var npgsqlParameter = (NpgsqlParameter) parameter;
                        npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
                    });
                persistence.ConnectionBuilder(() =>
                    new NpgsqlConnection("Server=localhost;Port=5432;Database=Test;User Id=admin;Password=admin;"));

                persistence.EnableTransactionalSession();

                endpointConfiguration.EnableOutbox();

                var pipeline = endpointConfiguration.Pipeline;
                pipeline.Register(
                    stepId: "first-step",
                    behavior: typeof(NServiceBusMessageProcessorPipelineBehavior),
                    description: "set processed messages");
                
                return endpointConfiguration;
            });
    }

    public static void AddEf(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(services =>
        {
            var session = services.GetRequiredService<ISqlStorageSession>();

            var dbConf = new DbContextOptionsBuilder<WalletDbContext>();
            
            dbConf.UseNpgsql(session.Connection);

            var context = new WalletDbContext(dbConf.Options);
            
            context.Database.UseTransaction(session.Transaction);

            session.OnSaveChanges((s, c) => context.SaveChangesAsync(c));

            return context;
        });
    }
}