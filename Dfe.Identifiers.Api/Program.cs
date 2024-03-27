using Dfe.Identifiers.Api.Context;
using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration config)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped<ITrustRepository, TrustRepository>();
    services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
    services.AddDbContext<MstrContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
}