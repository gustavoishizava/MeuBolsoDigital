using System.Reflection;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Interfaces;
using MBD.Transactions.Application.Services;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Infrastructure;
using MBD.Transactions.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.Transactions.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services
                .AddAppServices()
                .AddRepositories();

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
    }
}