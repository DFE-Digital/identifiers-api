using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Api.Services;
using Dfe.Identifiers.Application;
using Dfe.Identifiers.Domain.Authentication;
using Dfe.Identifiers.Infrastructure.Context;
using Dfe.Identifiers.Infrastructure.Interfaces;
using Dfe.Identifiers.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Dfe.Identifiers.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = AuthenticationConstants.APIKEYNAME,
                Description = "A valid ApiKey in the 'ApiKey' header is required to " +
                              "access the Academies API.",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                In = ParameterLocation.Header
            };
        
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {{ securityScheme, new List<string>() }});
        });
        services.AddScoped<ITrustRepository, TrustRepository>();
        services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
        services.AddScoped<IIdentifiersQuery, IdentifiersQuery>();
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddDbContext<MstrContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}