
using Infrastructure;
using Shared.Infrastructure;
using Shared.Infrastructure.Configs.Swagger;
// using SharedLibrary.Utils;

namespace ClothingStore.BE.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<SwaggerConfigSetup>();
            var app = builder.Build();

            // using (var scope = app.Services.CreateScope())
            // {
            //     var migrator = scope.ServiceProvider.GetRequiredService<AutoMigration>();

            //     if (app.Environment.IsDevelopment())
            //     {
            //         // In dev: generate a new migration and apply it
            //         migrator.GenerateMigration();
            //         migrator.ApplyMigration();
            //     }
            //     else
            //     {
            //         // In prod: only apply existing migrations
            //         migrator.ApplyMigration();
            //     }
            // }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(SwaggerUIConfig.ConfigureSwaggerUI);
            }
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
