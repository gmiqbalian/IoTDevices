using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(config => config.AddJsonFile("local.settings.json"))
    .ConfigureServices(services =>
    {
        services.AddDbContext<CosmosDbContext>(
            x => x.UseCosmos(config.Configuration.GetConnectionString("CosmosDb")!, "gm"));
    })
    .Build();

host.Run();
