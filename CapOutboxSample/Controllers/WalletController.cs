using CapOutboxSample.Constants;
using CapOutboxSample.Data;
using CapOutboxSample.Events;
using CapOutboxSample.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CapOutboxSample.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ICapPublisher _capBus;
        private readonly WalletDbContext _dbContext;

        public WalletController(ICapPublisher capBus, WalletDbContext dbContext)
        {
            _capBus = capBus;
            _dbContext = dbContext;
        }
        
        [HttpPost("basic")]
        public IActionResult Post([FromBody] PostWalletRequest request)
        {
            _dbContext.Wallet.Add(new Wallet
            {
                Username = request.Username,
                Balance = request.Balance
            });
            var queueHeaders = new Dictionary<string, string>()
            {
                {"correlationId", Guid.NewGuid().ToString()}
            };

            var @event = new WalletCreated {Username = request.Username, Balance = request.Balance};

            _capBus.Publish(EventConstants.WalletCreated, @event, queueHeaders);

            _dbContext.SaveChanges();

            return Created(string.Empty,default);
        }
        

        [HttpPost("outbox")]
        public IActionResult OutboxPost([FromBody] PostWalletRequest request)
        {
            var transactionId = Guid.NewGuid().ToString();
            
            using var transaction = _dbContext.Database.BeginTransaction(_capBus, autoCommit: false);

            var wallet = new Wallet
            {
                Username = request.Username,
                Balance = request.Balance
            };
            
            _dbContext.Wallet.Add(wallet);
            
            var queueHeaders = new Dictionary<string, string>()
            {
                {"correlationId", transactionId}
            };
            
            var @event = new WalletCreated {Username = request.Username, Balance = request.Balance};
            
            _capBus.Publish(EventConstants.WalletCreated, @event, queueHeaders);

            _dbContext.SaveChanges();

            wallet.Balance += 10;
            _dbContext.Wallet.Update(wallet);
            _dbContext.SaveChanges();

            transaction.Commit();

            return Created(string.Empty,default);
        }
        
        [HttpPost("withcallback")]
        public IActionResult PostWithCallback([FromBody] PostWalletRequest request)
        {
            using var transaction = _dbContext.Database.BeginTransaction(_capBus, autoCommit: false);
            
            _dbContext.Wallet.Add(new Wallet
            {
                Username = request.Username,
                Balance = request.Balance
            });

            var @event = new WalletCreated {Username = request.Username, Balance = request.Balance};

            _capBus.Publish(EventConstants.WalletCreated, @event, EventConstants.WalletCreatedCallback);

            _dbContext.SaveChanges();

            transaction.Commit();

            return Created(string.Empty, default);
        }
    }
}