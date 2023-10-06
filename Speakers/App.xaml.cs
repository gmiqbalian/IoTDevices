using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Speakers
{
    public partial class App : Application
    {
        private static IHost? _speakerAppHost { get; set; }
        public App()
        {
            _speakerAppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("Speakers")));
                    services.AddSingleton<INetworkService, NetworkService>();
                    services.AddSingleton<IDeviceService, DeviceService>();
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _speakerAppHost!.StartAsync();

            var mainWindow = _speakerAppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
