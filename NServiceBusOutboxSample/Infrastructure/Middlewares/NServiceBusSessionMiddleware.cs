using NServiceBus.TransactionalSession;

namespace NServiceBusOutboxSample.Infrastructure.Middlewares;

public class NServiceBusSessionMiddleware
{
    private readonly RequestDelegate next;

    public NServiceBusSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext httpContext, ITransactionalSession session)
    {
        await session.Open(new SqlPersistenceOpenSessionOptions());

        await next(httpContext);

        await session.Commit();
    }
}