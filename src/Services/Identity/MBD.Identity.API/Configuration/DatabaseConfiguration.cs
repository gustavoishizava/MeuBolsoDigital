using MBD.Identity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.Identity.API.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddEFContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
                options.UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}