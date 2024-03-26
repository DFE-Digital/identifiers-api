using System.Net.Mime;
using Dfe.Identifiers.Api.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dfe.Identifiers.Api.Test
{
    public class ApiTestFixture : IDisposable
    {
        // private const string ConnectionStringKey = "ConnectionStrings:DefaultConnection";
        private const string connectionString = "Server=localhost,1433;Database=ApiTests;User Id=sa;TrustServerCertificate=True;Password=StrongPassword905";
        private DbContextOptions<MstrContext> _dbContextOptions { get; init; }

        public ApiTestFixture()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MstrContext>()
                .UseSqlServer(connectionString)
                .Options;
            using var context = GetMstrContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        public MstrContext GetMstrContext() => new MstrContext(_dbContextOptions);

        public void Dispose()
        {
        }

        // private static string BuildDatabaseConnectionString(IConfigurationBuilder config)
        // {
        //     var currentConfig = config.Build();
        //     var connection = currentConfig[ConnectionStringKey];
        //     var sqlBuilder = new SqlConnectionStringBuilder(connection);
        //     sqlBuilder.InitialCatalog = "ApiTests";
        //
        //     var result = sqlBuilder.ToString();
        //
        //     return result;
        // }
    }
}