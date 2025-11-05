using Microsoft.OpenApi.Models;
using Products.Application.Interfaces;
using Products.Application.Services;
using Products.Domain.Repositories;
using Products.Infrastructure.Repositories;
using Products.Infrastructure.Security;

namespace Products.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<DocumentProcessingService>();
        services.AddSingleton<IFileDataExtractor, FileDataExtractor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IProductsDataProcessor, ProductsDataProcessor>();
        services.AddSingleton<IPasswordVerifier, PlainTextPasswordVerifier>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });

            var basicScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic authentication"
            };

            c.AddSecurityDefinition("basic", basicScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}