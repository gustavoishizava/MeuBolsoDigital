using System.Reflection;
using System.Text.Json.Serialization;
using MBD.BankAccounts.API.Configuration;
using MBD.BankAccounts.Infrastructure.Context;
using MBD.Core.Identity;
using MBD.IntegrationEventLog;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            services.AddDbContext<BankAccountContext>(options =>
            {
                options.UseInMemoryDatabase("BankAccountInMemory");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationEventLogInMemory");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddHealthCheckConfiguration();
            services.AddJwtConfiguration(Configuration);

            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.LowercaseUrls = true;
                routeOptions.LowercaseQueryStrings = true;
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.SuppressModelStateInvalidFilter = true;
                }).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddDomaindServices()
                    .AddAppServices()
                    .AddRepositories()
                    .AddConfigurations(Configuration)
                    .AddDomainEvents()
                    .AddIntegrationEventLogsService();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.Load("MBD.BankAccounts.Application"));

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
            var integrationContext = serviceProvider.GetRequiredService<IntegrationEventLogContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            integrationContext.Database.EnsureDeleted();
            integrationContext.Database.EnsureCreated();
        }
    }
}
