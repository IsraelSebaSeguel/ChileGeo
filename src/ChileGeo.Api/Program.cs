using ChileGeo.Api.Middleware;
using ChileGeo.Api.Security;
using ChileGeo.Api.Services;
using ChileGeo.DataAccess;
using ChileGeo.Domain.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Structured logging to console + rolling file (bonus: Registro de Log).
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/chilegeo-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Data access layer (Repository + Factory patterns) registered via composition root.
builder.Services.AddDataAccess();

// Application services (business logic, SRP) — depend on Domain abstractions only (DIP).
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IComunaService, ComunaService>();

// Cross-cutting security filter (bonus: Seguridad).
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ChileGeo API",
        Version = "v1",
        Description = "API REST para consultar y actualizar Regiones y Comunas de Chile."
    });
});

var app = builder.Build();

// Request logging goes outermost so it reports the final status code set by the exception middleware below.
app.UseSerilogRequestLogging();

// Centralized exception handling (Middleware pattern).
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
