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
    }
}