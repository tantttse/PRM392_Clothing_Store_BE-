using System.Text;
using Shared.Application.Abstractions.Authentication;
using ClothingStore.Application.Abstractions.UnitOfWork;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Common;
using ClothingStore.Infrastructure.Persistence.Contexts;
using ClothingStore.Infrastructure.Repositories;
using Shared.Infrastructure.Authentication;
using Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Application.Abstractions.UnitOfWork;
using Shared.Authentication;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Configs.Security;
using Shared.Infrastructure.Data.Interceptors;
// using SharedLibrary.Utils;

namespace Shared.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind JwtConfigs from appsettings.json
            services.Configure<JwtConfigs>(configuration.GetSection("JwtConfigs"));
            var jwtSettings = configuration.GetSection("JwtConfigs").Get<JwtConfigs>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISaveChangesUnitOfWork, UserUnitOfWork>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
            services.AddAutoMapper(typeof(ClothingStore.Application.Mappings.UserProfile).Assembly);
            services.AddScoped<ICompositeUnitOfWork, CompositeUnitOfWork>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new CurrentUserModelBinderProvider());
            });
            // var provider = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            // var logger = provider.CreateLogger<AutoMigration>();

            // var migrator = new AutoMigration(logger);
            // migrator.GenerateMigration();
            // migrator.ApplyMigration();
            return services;
        }
    }
}