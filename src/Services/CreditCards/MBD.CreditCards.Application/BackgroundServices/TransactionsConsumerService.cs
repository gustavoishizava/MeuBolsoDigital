using System;
using System.Threading;
using System.Threading.Tasks;
using MBD.CreditCards.Application.IntegrationEvents.Events;
using MBD.MessageBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MBD.CreditCards.Application.BackgroundServices
{
    public class TransactionsConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionsConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TransactionsConsumerService(IMessageBus messageBus, ILogger<TransactionsConsumerService> logger, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("========= Serviço em execução. =========");

            _messageBus.SubscribeAsync<TransactionLinkedToCreditCardBillIntegrationEvent>(
                subscriptionId: "TransactionLinkedToCreditCardBillIntegrationEvent",
                onMessage: async request => await AddTransaction(request));

            return Task.CompletedTask;
        }

        private async Task AddTransaction(TransactionLinkedToCreditCardBillIntegrationEvent @event)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(@event);
        }
    }
}