using IoTDevicesAzureFunctions.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((config, services) =>
    {
        services.AddDbContext<CosmosDbContext>(x => x.UseCosmos(config.Configuration.GetConnectionString("CosmosDb")!, "gm-cosmosdb"));
    })
    .Build();

host.Run();
