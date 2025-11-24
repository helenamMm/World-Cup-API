using MongoDB.Driver.Core.Operations;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Pkix;
using WorldCupProjectApi.Services;
using WorldCupProjectApi.Middlewares;

namespace WorldCupProjectApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<MongoDbService>();
        builder.Services.AddSingleton<UsuarioService>();
        builder.Services.AddSingleton<EquipoService>();
        builder.Services.AddSingleton<PartidoService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<NotificationService>();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c => 
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "WorldCupProjectApi", 
                Version = "v1",
                Description = "API para el proyecto del Mundial de Fútbol"
            });
            
            // Add JWT Bearer token support to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        var app = builder.Build();
        app.UseCors("AllowAll");
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<AuthMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}