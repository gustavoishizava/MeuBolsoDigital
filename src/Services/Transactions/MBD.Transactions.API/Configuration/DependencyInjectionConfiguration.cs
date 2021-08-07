using System;
using System.Reflection;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.API.Configuration.HttpClient;
using MBD.Transactions.Application.Commands;
using MBD.Transactions.Application.DomainEventHandlers;
using MBD.Transactions.Application.Interfaces;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Application.Services;
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
                .AddAppServices()
                .AddRepositories()
                .AddHttpClients(configuration)
                .AddCommands()
                .AddDomainEvents();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddAutoMapper(Assembly.Load("MBD.Transactions.Application"));

            return services;
        }

        private static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryAppService, CategoryAppService>();

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

            return services;
        }

        private static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<TransactionCreatedDomainEvent>, TransactionCreatedDomainEventHandler>();

            return services;
        }
    }
}