using Application.Abstractions.Authentication;
using ClothingStore.Application.Abstractions.UnitOfWork;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Common;
using ClothingStore.Infrastructure.Persistence.Contexts;
using ClothingStore.Infrastructure.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions.Authentication;
using Shared.Application.Abstractions.UnitOfWork;
using Shared.Authentication;
using Shared.Infrastructure.Authentication;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Configs.Security;
// using SharedLibrary.Utils;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind JwtConfigs from appsettings.json
            services.Configure<JwtConfigs>(configuration.GetSection("JwtConfigs"));

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddDbContext<UsersDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToArray();
                if (interceptors.Any())
                {
                    options.AddInterceptors(interceptors);
                }
            });

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaveChangesUnitOfWork, UserUnitOfWork>();
            services.AddAutoMapper(typeof(ClothingStore.Application.Mappings.UserProfile).Assembly);
            services.AddScoped<ICompositeUnitOfWork, CompositeUnitOfWork>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            // var provider = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            // var logger = provider.CreateLogger<AutoMigration>();

            // var migrator = new AutoMigration(logger);
            // migrator.GenerateMigration();
            // migrator.ApplyMigration();
            return services;
        }
    }
}