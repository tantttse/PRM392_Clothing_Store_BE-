using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Commands;
using FluentValidation;
using ClothingStore.Application.Behaviors;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services , IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            var sharedLibraryAssembly = typeof(SaveChangesCommandHandler).Assembly;

            services.AddMediatR(configuration => 
            {
                configuration.RegisterServicesFromAssembly(assembly);
                configuration.RegisterServicesFromAssembly(sharedLibraryAssembly);
            });
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return services;
        }
    }
}