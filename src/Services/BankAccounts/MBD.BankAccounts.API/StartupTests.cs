using MBD.BankAccounts.API.Configuration;
using MBD.BankAccounts.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MBD.BankAccounts.API
{
    public class StartupTests
    {
        public StartupTests(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEFContextConfiguration(Configuration);
            services.AddApiConfiguration();

            Seed(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiConfiguration(env);
        }

        public static void Seed(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<BankAccountContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
