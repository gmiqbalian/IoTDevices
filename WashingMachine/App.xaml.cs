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

namespace WashingMachine
{
    public partial class App : Application
    {
        private static IHost _washingMachineAppHost { get; set; }
        public App()
        {
            _washingMachineAppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional:true, reloadOnChange:true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("WashingMachine")));
                    services.AddSingleton<NetworkService>();
                    services.AddSingleton<DeviceService>();
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _washingMachineAppHost!.StartAsync();

            var mainWindow = _washingMachineAppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
