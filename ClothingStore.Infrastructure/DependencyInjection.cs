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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Application.Abstractions.UnitOfWork;
using Shared.Authentication;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Configs.Security;
using Shared.Infrastructure.Data.Interceptors;
using Shared.Domain.Common.Exceptions.Handler;
using Application.Abstractions.Payment;
using Infrastructure.Payments;
using Shared.Infrastructure.Configs.Payment;
// using SharedLibrary.Utils;

namespace Shared.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // UPDATED: Changed from direct Configure to ConfigureOptions for strongly-typed binding
            // OLD: services.Configure<ErrorHandlingConfigs>(configuration.GetSection("ErrorHandling"));
            services.ConfigureOptions<ErrorHandlingConfigSetup>();
            services.ConfigureOptions<JwtConfigSetup>();
            services.ConfigureOptions<VnPayConfigSetup>();

            // REMOVED: Direct configuration binding - now handled by ConfigureOptions above
            // OLD CODE:
            // services.Configure<JwtConfigs>(configuration.GetSection("JwtConfigs"));
            // var jwtSettings = configuration.GetSection("JwtConfigs").Get<JwtConfigs>();
            // var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            // UPDATED: JWT Authentication - Split into two parts for proper DI injection
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
                    // REMOVED: IssuerSigningKey, ValidIssuer, ValidAudience - now set via AddOptions below
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // ADDED: Configure JWT options after authentication is added (proper DI pattern)
            // This allows injecting IOptions<JwtConfigs> to configure JWT Bearer options
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtConfigs>>((options, jwtConfigs) =>
                {
                    var jwtSettings = jwtConfigs.Value;
                    var key = Encoding.UTF8.GetBytes(jwtSettings.Secret); // UPDATED: Changed from ASCII to UTF8

                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                    options.TokenValidationParameters.ValidIssuer = jwtSettings.Issuer;
                    options.TokenValidationParameters.ValidAudience = jwtSettings.Audience;
                });

            // EF Core + interceptors
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>(); // MOVED: From line 82 to here for better organization
            services.AddDbContext<UsersDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToArray();
                if (interceptors.Any())
                {
                    options.AddInterceptors(interceptors);
                }
            });

            // Application services
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISaveChangesUnitOfWork, UserUnitOfWork>();
            // REMOVED: services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>(); - Moved to line 69
            services.AddScoped<ICompositeUnitOfWork, CompositeUnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // AutoMapper
            services.AddAutoMapper(typeof(ClothingStore.Application.Mappings.UserProfile).Assembly);

            // Controllers + model binders
            services.AddHttpContextAccessor();
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new CurrentUserModelBinderProvider());
            });

            // Exception handling
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddProblemDetails(); // optional, for ProblemDetails support

            // REMOVED: Auto migration code - Should be handled separately
            // OLD CODE:
            // var provider = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            // var logger = provider.CreateLogger<AutoMigration>();
            // var migrator = new AutoMigration(logger);
            // migrator.GenerateMigration();
            // migrator.ApplyMigration();

            return services;
        }
    }
}