using IntergalacticLogistics.Domain;
using JasperFx;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace IntergalacticLogistics.Infrastructure.Persistence;

public static class MartenDocumentStore
{
    public static IServiceCollection AddMartenDocumentStore(this IServiceCollection services,IConfiguration configuration,bool isDevelopment = false)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required in appsettings.json");

        services.AddMarten(options =>
        {
            options.Connection(connectionString);

            options.Schema.For<Shipment>().Identity(x => x.Id);

            //for development 
            if (isDevelopment)
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
        })

        .UseLightweightSessions();

        return services;
    }
}

