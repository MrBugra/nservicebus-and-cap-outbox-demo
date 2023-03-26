// using CapOutboxSample.Data;
// using DotNetCore.CAP;
//
// namespace CapOutboxSample.Infrastructure.Middlewares;
//
// public class CapSessionMiddleware
// {
//     private readonly RequestDelegate _next;
//
//     public CapSessionMiddleware(RequestDelegate next)
//     {
//         this._next = next;
//     }
//
//     public async Task Invoke(HttpContext httpContext, ICapPublisher capbus, WalletDbContext dbContext)
//     {
//         await using var transaction = await dbContext.Database.BeginTransactionAsync(capbus, autoCommit: false);
//
//         await _next(httpContext);
//
//         await dbContext.SaveChangesAsync();
//         await transaction.CommitAsync();
//     }
// }