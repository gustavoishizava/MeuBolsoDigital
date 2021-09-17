using System;
using System.Reflection;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.MessageBus;
using MBD.Transactions.API.Configuration.HttpClient;
using MBD.Transactions.API.Consumers;
using MBD.Transactions.Application.Commands;
using MBD.Transactions.Application.DomainEventHandlers;
using MBD.Transactions.Application.IntegrationEvents.EventHandling;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Application.Queries;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Events;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Domain.Interfaces.Services;
using MBD.Transactions.Infrastructure;
using MBD.Transactions.Infrastructure.Repositories;
using MBD.Transactions.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MBD.Transactions.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRepositories()
                .AddHttpClients(configuration)
                .AddCommands()
                .AddQueries()
                .AddDomainEvents()
                .AddIntegrationEvents()
                .AddConfigurations(configuration)
                .AddMessageBus()
                .AddConsumers();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddAutoMapper(Assembly.Load("MBD.Transactions.Application"));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IBankAccountService, BankAccountService>(config =>
            {
                config.BaseAddress = new Uri(configuration["UrlServices:BankAccountService"]);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(PollyRetryConfiguration.WaitToRetry())
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetCallingAssembly());

            services.AddScoped<IRequestHandler<CreateTransactionCommand, IResult<TransactionResponse>>, CreateTransactionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateTransactionCommand, IResult>, UpdateTransactionCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteTransactionCommand, IResult>, DeleteTransactionCommandHandler>();

            services.AddScoped<IRequestHandler<CreateCategoryCommand, IResult<CategoryResponse>>, CreateCategoryCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand, IResult>, UpdateCategoryCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryCommand, IResult>, DeleteCategoryCommandHandler>();

            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddScoped<ITransactionQuery, TransactionQuery>();
            services.AddScoped<ICategoryQuery, CategoryQuery>();

            return services;
        }

        private static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<TransactionCreatedDomainEvent>, TransactionCreatedDomainEventHandler>();
            services.AddScoped<INotificationHandler<TransactionUpdatedDomainEvent>, TransactionUpdatedDomainEventHandler>();
            services.AddScoped<INotificationHandler<TransactionDeletedDomainEvent>, TransactionDeletedDomainEventHandler>();
            services.AddScoped<INotificationHandler<RealizedPaymentDomainEvent>, RealizedPaymentDomainEventHandler>();
            services.AddScoped<INotificationHandler<ReversedPaymentDomainEvent>, ReversedPaymentDomainEventHandler>();
            services.AddScoped<INotificationHandler<ValueChangedDomainEvent>, ValueChangedDomainEventHandler>();

            services.AddScoped<INotificationHandler<CategoryNameChangedDomainEvent>, CategoryNameChangedDomainEventHandler>();

            return services;
        }

        private static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<TransactionPaidIntegrationEvent>, TransactionPaidIntegrationEventHandler>();
            services.AddScoped<INotificationHandler<TransactionUndoPaymentIntegrationEvent>, TransactionUndoPaymentIntegrationEventHandler>();
            services.AddScoped<INotificationHandler<TransactionValueChangedIntegrationEvent>, TransactionValueChangedIntegrationEventHandler>();
            services.AddScoped<INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>, BankAccountDescriptionIntegrationEventHandler>();

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)));

            return services;
        }

        private static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MBD.MessageBus.MessageBus>();

            return services;
        }

        private static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<BankAccountConsumer>();

            return services;
        }
    }
}