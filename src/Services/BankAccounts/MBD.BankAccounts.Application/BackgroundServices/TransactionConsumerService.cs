using System;
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Application.IntegrationEvents;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Domain.Interfaces.Services;
using MBD.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MBD.BankAccounts.Application.BackgroundServices
{
    public class TransactionConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TransactionConsumerService(IMessageBus messageBus, ILogger<TransactionConsumerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("========= Serviço em execução. =========");

            SetupChannel();

            _messageBus.SubscribeAsync<TransactionPaidIntegrationEvent>(
                nameof(TransactionPaidIntegrationEvent),
                async request => await AddTransactionAsync(request));

            _messageBus.SubscribeAsync<TransactionUndoPaymentIntegrationEvent>(
                nameof(TransactionUndoPaymentIntegrationEvent),
                async request => await RemoveTransactionAsync(request));

            return Task.CompletedTask;
        }

        private void SetupChannel()
        {
            _messageBus.TryConnect();

            _messageBus.Channel.QueueDeclare(
                queue: nameof(TransactionPaidIntegrationEvent),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _messageBus.Channel.QueueDeclare(
                queue: nameof(TransactionUndoPaymentIntegrationEvent),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        private async Task AddTransactionAsync(TransactionPaidIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var transactionManagementService = scope.ServiceProvider.GetRequiredService<ITransactionManagementService>();

            await transactionManagementService.AddTransactionToAccountAsync(
                message.BankAccountId,
                message.Id,
                message.Value,
                Enum.Parse<TransactionType>(message.Type),
                message.Date);
        }

        private async Task RemoveTransactionAsync(TransactionUndoPaymentIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITransactionManagementService>();

            await service.RemoveTransactionAsync(message.Id);
        }
    }
}