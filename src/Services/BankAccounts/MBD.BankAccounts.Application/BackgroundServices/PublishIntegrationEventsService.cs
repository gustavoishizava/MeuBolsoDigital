using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Extensions;
using MBD.IntegrationEventLog.Services;
using MBD.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MBD.BankAccounts.Application.BackgroundServices
{
    public class PublishIntegrationEventsService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PublishIntegrationEventsService> _logger;

        public PublishIntegrationEventsService(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<PublishIntegrationEventsService> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de publicação de mensagens iniciado.");

            SetupChannel();

            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishEventsAsync();

                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation("Serviço de publicação de mensagens parando...");
        }

        private void SetupChannel()
        {
            _messageBus.TryConnect();

            string queueTransactions = "bank_accounts.transactions";
            string queueCreditCards = "bank_accounts.credit_cards";

            string exchange = "bank_accounts.direct";

            string[] routingKeys = new[] { "created", "updated", "deleted" };

            _messageBus.Channel.QueueDeclare(
                queue: queueTransactions,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _messageBus.Channel.QueueDeclare(
                queue: queueCreditCards,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _messageBus.Channel.ExchangeDeclare(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            for (int i = 0; i < routingKeys.Length; i++)
            {
                _messageBus.Channel.QueueBind(queueTransactions, exchange, routingKeys[i]);
                _messageBus.Channel.QueueBind(queueCreditCards, exchange, routingKeys[i]);
            }
        }

        private async Task PublishEventsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var integrationEventLogService = scope.ServiceProvider.GetRequiredService<IIntegrationEventLogService>();

            var events = await integrationEventLogService.RetrieveEventLogsPendingToPublishAsync();
            if (events.IsNullOrEmpty())
                return;

            foreach (var @event in events)
            {
                try
                {
                    var message = JsonSerializer.Deserialize<object>(@event.Content);
                    if (message is null)
                        continue;

                    _messageBus.Publish(message, @event.EventTypeName, "bank_accounts.direct");

                    await integrationEventLogService.RemoveEventAsync(@event);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}