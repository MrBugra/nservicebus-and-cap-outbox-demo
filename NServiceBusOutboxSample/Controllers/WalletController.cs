using Microsoft.AspNetCore.Mvc;
using NServiceBus.TransactionalSession;
using NServiceBusOutboxSample.Data;
using NServiceBusOutboxSample.Events;
using NServiceBusOutboxSample.Models;

namespace NServiceBusOutboxSample.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ITransactionalSession _session;
        private readonly WalletDbContext _dbContext;

        public WalletController(ITransactionalSession session, WalletDbContext dbContext)
        {
            _session = session;
            _dbContext = dbContext;
        }

        [HttpPost("outbox")]
        public async Task<IActionResult> PostOutbox([FromBody] PostWalletRequest request)
        {
            var wallet = new Wallet
            {
                Username = request.Username,
                Balance = request.Balance
            };
            
            _dbContext.Wallet.Add(wallet);
            await _dbContext.SaveChangesAsync();

            var @event = new WalletCreated {Username = request.Username, Balance = request.Balance};

            wallet.Balance += 10;
            _dbContext.Wallet.Update(wallet);

            await _session.SendLocal(@event);

            return Created(string.Empty, default);
        }
    }
}
