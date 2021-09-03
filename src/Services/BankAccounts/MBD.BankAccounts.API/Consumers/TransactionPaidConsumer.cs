using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Application.IntegrationEvents;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Domain.Interfaces.Services;
using MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MBD.BankAccounts.API.Consumers
{
    public class TransactionPaidConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<TransactionPaidConsumer> _logger;
        private readonly string _queueName;

        private readonly IServiceProvider _serviceProvider;

        public TransactionPaidConsumer(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<TransactionPaidConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            var rabbitMqConfiguration = rabbitMqOptions.Value;
            _queueName = nameof(TransactionPaidIntegrationEvent);
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqConfiguration.HostName,
                UserName = rabbitMqConfiguration.UserName,
                Password = rabbitMqConfiguration.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _serviceProvider = serviceProvider;

            _logger.LogInformation("========= Conectando ao RabbitMQ. =========");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("========= Serviço em execução. =========");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                _logger.LogInformation("========= Recebendo mensagem de integração =========");

                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonSerializer.Deserialize<TransactionPaidIntegrationEvent>(contentString, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                try
                {
                    _logger.LogInformation(contentString);

                    using var scope = _serviceProvider.CreateScope();
                    var transactionManagementService = scope.ServiceProvider.GetRequiredService<ITransactionManagementService>();

                    await transactionManagementService.AddTransactionToAccountAsync(
                        message.BankAccountId,
                        message.Id,
                        message.Value,
                        Enum.Parse<TransactionType>(message.Type),
                        message.Date);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);

                    throw;
                }
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }
    }
}