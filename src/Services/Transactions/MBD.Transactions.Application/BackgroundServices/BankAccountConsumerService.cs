using System;
using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MBD.Transactions.Application.BackgroundServices
{
    public class BankAccountConsumerService : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BankAccountConsumerService> _logger;
        private const string QueueName = "bank_accounts.transactions";
        private const string BankAccountExchange = "bank_accounts.topic";

        public BankAccountConsumerService(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<BankAccountConsumerService> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço em execução.");

            SetupChannel();

            _messageBus.SubscribeAsync(
                subscriptionId: QueueName,
                async (object s, BasicDeliverEventArgs args) =>
                {
                    try
                    {
                        var routingKey = args.RoutingKey;

                        switch (routingKey)
                        {
                            case "created":
                                await CreateBankAccountAsync(args.Body.GetMessage<BankAccountCreatedIntegrationEvent>());
                                break;

                            case "updated":
                                await SetDescriptionAsync(args.Body.GetMessage<BankAccountDescriptionChangedIntegrationEvent>());
                                break;

                            default:
                                break;
                        }

                        _messageBus.Channel.BasicAck(args.DeliveryTag, false);
                    }
                    catch
                    {
                        _messageBus.Channel.BasicNack(args.DeliveryTag, false, true);
                        _logger.LogError($"Erro ao processar mensagem:.");
                        throw;
                    }
                });

            return Task.CompletedTask;
        }

        private void SetupChannel()
        {
            _messageBus.TryConnect();

            string[] routingKeys = new[] { "created", "updated", "deleted" };

            _messageBus.Channel.QueueDeclare(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _messageBus.Channel.ExchangeDeclare(
                exchange: BankAccountExchange,
                type: ExchangeType.Topic,
                durable: false,
                autoDelete: false,
                arguments: null
            );

            for (int i = 0; i < routingKeys.Length; i++)
            {
                _messageBus.Channel.QueueBind(QueueName, BankAccountExchange, routingKeys[i]);
            }
        }

        private async Task CreateBankAccountAsync(BankAccountCreatedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(message);
        }

        private async Task SetDescriptionAsync(BankAccountDescriptionChangedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(message);
        }
    }
}