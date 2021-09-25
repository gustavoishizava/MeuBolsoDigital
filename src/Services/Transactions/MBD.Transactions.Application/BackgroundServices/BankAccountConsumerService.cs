using System;
using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MBD.Transactions.Application.BackgroundServices
{
    public class BankAccountConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BankAccountConsumerService> _logger;

        public BankAccountConsumerService(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<BankAccountConsumerService> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço em execução.");

            _messageBus.SubscribeAsync<BankAccountDescriptionChangedIntegrationEvent>(
                subscriptionId: nameof(BankAccountDescriptionChangedIntegrationEvent),
                async request => await SetDescription(request));

            return Task.CompletedTask;
        }

        private async Task SetDescription(BankAccountDescriptionChangedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(message);
        }
    }
}