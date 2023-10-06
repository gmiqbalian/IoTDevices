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

namespace Television
{
    public partial class App : Application
    {
        public static IHost? appHost { get; set; }
        public App()
        {
            appHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("DeviceConnectionString")));
                    services.AddSingleton<DeviceService>();
                    services.AddSingleton<NetworkService>();

                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await appHost!.StartAsync();

            var mainWindow = appHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
                       
            base.OnStartup(e);
        }
    }
}
