using System.Net.Mime;
using Dfe.Identifiers.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dfe.Identifiers.Api.Test
{
    public class ApiTestFixture : IDisposable
    {
        private const string connectionStringKey = "ConnectionStrings:Default";
        private DbContextOptions<MstrContext> _dbContextOptions { get; init; }

        public ApiTestFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<ApiTestFixture>();

            Configuration = builder.Build();
            _dbContextOptions = new DbContextOptionsBuilder<MstrContext>()
                .UseSqlServer(Configuration[connectionStringKey])
                .Options;
            using var context = GetMstrContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        private IConfigurationRoot Configuration { get; init; }

        public MstrContext GetMstrContext() => new(_dbContextOptions);

        public void Dispose()
        {
        }
    }
}