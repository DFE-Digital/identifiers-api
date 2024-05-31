using System.Reflection;
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
        services.AddApplicationInsightsTelemetry();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
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
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        services.AddScoped<ITrustRepository, TrustRepository>();
        services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
        services.AddScoped<IIdentifiersQuery, IdentifiersQuery>();
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddDbContext<MstrContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MstrConnection") ?? configuration.GetConnectionString("DefaultConnection")));
        services.AddDbContext<AcademisationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AcademisationConnection") ?? configuration.GetConnectionString("DefaultConnection")));
    }
}