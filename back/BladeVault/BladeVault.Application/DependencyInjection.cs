using BladeVault.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BladeVault.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Реєструємо MediatR — знаходить всі Handler-и автоматично
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(assembly));

            // Реєструємо всі Validator-и автоматично
            services.AddValidatorsFromAssembly(assembly);

            // Реєструємо Behaviors — порядок важливий!
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingBehavior<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
