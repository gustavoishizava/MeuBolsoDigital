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
    public class BankAccountsConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BankAccountsConsumerService> _logger;

        public BankAccountsConsumerService(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<BankAccountsConsumerService> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("========= Serviço em execução. =========");

            _messageBus.SubscribeAsync<BankAccountCreatedIntegrationEvent>(
                nameof(BankAccountCreatedIntegrationEvent),
                async request => await AddBankAccount(request));

            return Task.CompletedTask;
        }

        private async Task AddBankAccount(BankAccountCreatedIntegrationEvent @event)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(@event);
        }
    }
}