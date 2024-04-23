using Dfe.Identifiers.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dfe.Identifiers.Api.Test
{
    public class ApiTestFixture : IDisposable
    {
        private const string connectionStringKey = "ConnectionStrings:DefaultConnection";
        private DbContextOptions<MstrContext> _mstrContextOptions { get; init; }
        
        private DbContextOptions<AcademisationContext> _academisationContextOptions { get; init; }

        public ApiTestFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<ApiTestFixture>();

            Configuration = builder.Build();
            _mstrContextOptions = new DbContextOptionsBuilder<MstrContext>()
                .UseSqlServer(Configuration[connectionStringKey])
                .Options;
            _academisationContextOptions = new DbContextOptionsBuilder<AcademisationContext>()
                .UseSqlServer(Configuration[connectionStringKey])
                .Options;
            using var mstrContext = GetMstrContext();
            using var academisationContext = GetAcademisationContext();
            mstrContext.Database.EnsureDeleted();
            academisationContext.Database.EnsureDeleted();
            mstrContext.Database.Migrate();
            academisationContext.Database.Migrate();
        }

        private IConfigurationRoot Configuration { get; init; }

        public MstrContext GetMstrContext() => new(_mstrContextOptions);
        public AcademisationContext GetAcademisationContext() => new(_academisationContextOptions);

        public void Dispose()
        {
        }
    }
}