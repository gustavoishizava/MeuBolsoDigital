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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MBD.BankAccounts.Application.BackgroundServices
{
    public class TransactionConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private const string QueueName = "transactions.bank_accounts";
        private const string ExchangeName = "transactions.topic";

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

            _messageBus.SubscribeAsync(
                subscriptionId: QueueName,
                onReceived: async (object s, BasicDeliverEventArgs args) =>
                {
                    try
                    {
                        var routingKey = args.RoutingKey;

                        switch (routingKey)
                        {
                            case "realized_payment":
                                await AddTransactionAsync(args.Body.GetMessage<TransactionPaidIntegrationEvent>());
                                break;
                            case "reversed_payment":
                                await RemoveTransactionAsync(args.Body.GetMessage<TransactionUndoPaymentIntegrationEvent>());
                                break;
                            case "value_changed":
                            case "deleted":
                                break;
                        }

                        _messageBus.Channel.BasicAck(args.DeliveryTag, false);
                    }
                    catch
                    {
                        _messageBus.Channel.BasicNack(args.DeliveryTag, false, false);
                        _logger.LogError("Erro ao processar mensagem.");
                        throw;
                    }
                });

            return Task.CompletedTask;
        }

        private void SetupChannel()
        {
            _messageBus.TryConnect();

            string[] routingKeys = new[] { "realized_payment", "reversed_payment", "value_changed", "deleted" };

            _messageBus.Channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: false,
                autoDelete: false,
                arguments: null
            );

            _messageBus.Channel.QueueDeclare(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            for (var i = 0; i < routingKeys.Length; i++)
            {
                _messageBus.Channel.QueueBind(
                    queue: QueueName,
                    exchange: ExchangeName,
                    routingKey: routingKeys[i],
                    arguments: null);
            }
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