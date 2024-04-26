using System.Net.Mime;
using Dfe.Identifiers.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dfe.Identifiers.Api.Test
{
    public class ApiTestFixture : IDisposable
    {
        private const string connectionStringKey = "ConnectionStrings:DefaultConnection";
        private DbContextOptions<MstrContext> _mstrContextOptions { get; init; }
        private DbContextOptions<AcademisationContext> _academisationContextOptions { get; init; }
        private static readonly object _lock = new();
        private static bool isInitalised = false;
        public HttpClient Client { get; init; }
        
        private readonly WebApplicationFactory<Startup> _application;

        public ApiTestFixture()
        {
            lock (_lock)
            {
                if (!isInitalised)
                {
                    string? connectionString = null;
                    _application = new WebApplicationFactory<Startup>()
                        .WithWebHostBuilder(builder =>
                        {
                            builder.ConfigureAppConfiguration((context, config) =>
                            {
                                config.AddJsonFile("appsettings.json")
                                    .AddEnvironmentVariables()
                                    .AddUserSecrets<ApiTestFixture>();
                                var configuration = config.Build();
                                connectionString = configuration[connectionStringKey];
                            });
                        });
                    
                    Client = _application.CreateClient();
                    Client.DefaultRequestHeaders.Add("ApiKey", "app-key");
                    Client.DefaultRequestHeaders.Add("ContentType", MediaTypeNames.Application.Json);
                    Client.BaseAddress = new Uri("https://identifiers-api.com");
                    
                    _mstrContextOptions = new DbContextOptionsBuilder<MstrContext>()
                        .UseSqlServer(connectionString)
                        .EnableSensitiveDataLogging()
                        .Options;
                    _academisationContextOptions = new DbContextOptionsBuilder<AcademisationContext>()
                        .UseSqlServer(connectionString)
                        .EnableSensitiveDataLogging()
                        .Options;
                }
                SetupDatabase();
            }
            
        }

        public MstrContext GetMstrContext() => new(_mstrContextOptions);
        public AcademisationContext GetAcademisationContext() => new(_academisationContextOptions);

        public void Dispose()
        {
        }

        private void SetupDatabase()
        {
            using var mstrContext = GetMstrContext();
            using var academisationContext = GetAcademisationContext();
            mstrContext.Database.EnsureDeleted();
            academisationContext.Database.EnsureDeleted();
            mstrContext.Database.Migrate();
            academisationContext.Database.Migrate();
        }
    }
}