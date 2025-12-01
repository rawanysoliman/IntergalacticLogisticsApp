using IntergalacticLogistics.Api.Logging;
using IntergalacticLogistics.Api.Middleware;
using IntergalacticLogistics.Application.Commands;
using IntergalacticLogistics.Application.Dtos;
using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Application.Strategies;
using IntergalacticLogistics.Infrastructure.Persistence;
using IntergalacticLogistics.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Serilog;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Intergalactic Logistics API",
        Version = "v1",
        Description = "Star Wars intergalactic shipment management system"
    });
});

//Wolverine
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(CreateShipmentCommandHandler).Assembly);
});

// Configure Marten Infrastructure extension
builder.Services.AddMartenDocumentStore(
    builder.Configuration,
    builder.Environment.IsDevelopment());

// Register Repository
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<CreateShipmentProcessor>();
builder.Services.AddScoped<ICreateShipmentProcessor>(sp =>
{
    var inner = sp.GetRequiredService<CreateShipmentProcessor>();
    var logger = sp.GetRequiredService<ILogger<CargoValidationShipmentDecorator>>();
    return new CargoValidationShipmentDecorator(inner, logger);
});

// Register Strategy Pattern implementations
builder.Services.AddScoped<StandardSpeedStrategy>();
builder.Services.AddScoped<HyperdriveExpressStrategy>();
builder.Services.AddScoped<SmugglerRouteStrategy>();

// Register strategy factory
builder.Services.AddScoped<Func<string, IShippingCostStrategy>>(serviceProvider => key =>
{
    return key switch
    {
        "StandardSpeed" => serviceProvider.GetRequiredService<StandardSpeedStrategy>(),
        "HyperdriveExpress" => serviceProvider.GetRequiredService<HyperdriveExpressStrategy>(),
        "SmugglerRoute" => serviceProvider.GetRequiredService<SmugglerRouteStrategy>(),
        _ => serviceProvider.GetRequiredService<StandardSpeedStrategy>()
    };
});




// Register HttpClient for SwapiClient and register ISwapiClient interface
builder.Services.AddHttpClient<ISwapiClient, SwapiClient>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {

        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            // In production, allow same origin (frontend served from wwwroot)
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Logging
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});

builder.Services.AddWolverineHttp();

var app = builder.Build();

// Add middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intergalactic Logistics API v1");
    c.RoutePrefix = "swagger"; 
});

app.UseHttpsRedirection();

// CORS must be before UseAuthorization
app.UseCors("AngularPolicy");

// Serve static files from wwwroot (Angular build)
app.UseStaticFiles();

app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Map Wolverine HTTP endpoints
app.MapWolverineEndpoints();

// Fallback to index.html for Angular routing (must be last)
app.MapFallbackToFile("index.html");

app.Run();

