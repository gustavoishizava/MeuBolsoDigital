using System.Reflection;
using MBD.BankAccounts.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.BankAccounts.API.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddEFContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BankAccountContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"), builder =>
                {
                    builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                });
                options.UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}