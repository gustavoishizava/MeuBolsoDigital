using System.Reflection;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.CreditCards.Application.BackgroundServices;
using MBD.CreditCards.Application.IntegrationEvents.Events;
using MBD.CreditCards.Application.IntegrationEvents.Handlers;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Services;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Infrastructure;
using MBD.CreditCards.Infrastructure.Repositories;
using MBD.MessageBus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.CreditCards.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppServices()
                    .AddRepositories()
                    .AddConsumers()
                    .AddMessageBus()
                    .AddConfigurations(configuration)
                    .AddIntegrationEvents();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.Load("MBD.CreditCards.Application"));

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardAppService, CreditCardAppService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<TransactionsConsumerService>();
            services.AddHostedService<BankAccountsConsumerService>();

            return services;
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus.MessageBus>();

            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)));

            return services;
        }

        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<TransactionLinkedToCreditCardBillIntegrationEvent>, TransactionLinkedToCreditCardBillIntegrationEventHandler>();
            services.AddScoped<INotificationHandler<BankAccountCreatedIntegrationEvent>, BankAccountCreatedIntegrationEventHandler>();

            return services;
        }
    }
}