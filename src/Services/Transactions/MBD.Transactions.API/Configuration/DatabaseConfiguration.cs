using System.Reflection;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MBD.Transactions.API.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddEFContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TransactionContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"), builder =>
                {
                    builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                });
                options.UseSnakeCaseNamingConvention();
            });

            return services;
        }

        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TransactionDatabaseSettings>(configuration.GetSection("MongoDbSettings"));
            
            services.AddSingleton<ITransactionDatabaseSettings>(sp => 
                sp.GetRequiredService<IOptions<TransactionDatabaseSettings>>().Value);

            return services;
        }
    }
}