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

namespace MBD.BankAccounts.API.Consumers
{
    public class TransactionConsumer : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TransactionConsumer(IMessageBus messageBus, ILogger<TransactionConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("========= Serviço em execução. =========");

            _messageBus.SubscribeAsync<TransactionPaidIntegrationEvent>(
                nameof(TransactionPaidIntegrationEvent),
                async request => await AddTransactionAsync(request));

            _messageBus.SubscribeAsync<TransactionUndoPaymentIntegrationEvent>(
                nameof(TransactionUndoPaymentIntegrationEvent),
                async request => await RemoveTransactionAsync(request));

            return Task.CompletedTask;
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