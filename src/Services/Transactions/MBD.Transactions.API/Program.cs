using MBD.IntegrationEventLog;
using MBD.Transactions.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MBD.Transactions.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var serviceScope = host.Services.CreateScope();
            var service = serviceScope.ServiceProvider;
            var transactionContext = service.GetRequiredService<TransactionContext>();
            var integrationEventLogContext = service.GetRequiredService<IntegrationEventLogContext>();

            transactionContext.Database.Migrate();
            integrationEventLogContext.Database.Migrate();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
