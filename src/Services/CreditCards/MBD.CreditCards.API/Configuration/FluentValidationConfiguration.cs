using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace MBD.CreditCards.API.Configuration
{
    public static class FluentValidationConfiguration
    {
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidation(options =>
                options.RegisterValidatorsFromAssembly(Assembly.Load("MBD.CreditCards.Application")));

            services.AddFluentValidationRulesToSwagger();

            return services;
        }
    }
}