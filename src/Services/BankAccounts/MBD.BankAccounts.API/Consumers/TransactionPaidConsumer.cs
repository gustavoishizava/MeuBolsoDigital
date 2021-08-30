using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MBD.BankAccounts.API.Consumers
{
    public class TransactionPaidConsumer : BackgroundService
    {
        private IConnection _connection;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}