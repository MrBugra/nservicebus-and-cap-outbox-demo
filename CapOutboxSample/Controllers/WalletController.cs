using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapOutboxSample.Constants;
using CapOutboxSample.Data;
using CapOutboxSample.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("")]
        public IActionResult Post([FromBody] PostWalletRequest request)
        {
            var transactionId = Guid.NewGuid().ToString();
            
            using var transaction = _dbContext.Database.BeginTransaction(_capBus, autoCommit: false);
            
            _dbContext.Wallet.Add(new Wallet
            {
                Username = request.Username,
                Balance = request.Balance
            });
            var queueHeaders = new Dictionary<string, string>()
            {
                {"correlationId", transactionId}
            };

            _capBus.Publish(EventConstants.WalletCreated, request, queueHeaders);

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
            
            _capBus.Publish(EventConstants.WalletCreated, request, EventConstants.WalletCreatedCallback);

            _dbContext.SaveChanges();

            transaction.Commit();

            return Created(string.Empty, default);
        }
    }
}