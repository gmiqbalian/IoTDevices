using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WashingMachine;

public partial class MainWindow : Window
{
    private readonly NetworkService _networkService;
    private readonly DeviceService _deviceService;
    public MainWindow(NetworkService networkService, DeviceService deviceService)
    {
        InitializeComponent();

        _networkService = networkService;
        _deviceService = deviceService;

        Task.WhenAll(ConfigureDevice(),
            GetConnectionStatusAsync(),
            ToggleMachineState(),
            SendTelemetryDataAsync());
    }
    private async Task ConfigureDevice()
    {
        var twinCollection = new TwinCollection();
        twinCollection["deviceType"] = "WashingMachine";
        twinCollection["location"] = "Washroom";

        if (_deviceService.IsConfigured)
        {
            await _deviceService.UpdateTwinAsync(twinCollection);
            await _deviceService.RegisterDirectMethodToCloud();
        }
    }
    private async Task ToggleMachineState()
    {
        Storyboard machine = (Storyboard)this.FindResource("MachineStoryboard");
        
        while (true)
        {
            if (_deviceService.IsSendingAllowed)
                machine.Begin();
            else
                machine.Stop();

            await Task.Delay(5000);
        }
    }
    private async Task GetConnectionStatusAsync()
    {
        while (true)
        {
            var status = await _networkService.TestConnectivityAsync();
            ConnectivityStatus.Text = status;

            if (status.ToLower() == "connected")
                ConnectivityStatus.Background = Brushes.Green;
            else if (status.ToLower() == "disconnected")
                ConnectivityStatus.Background = Brushes.Red;

            await Task.Delay(1000);
        }
    }
    private async Task SendTelemetryDataAsync()
    {
        while (true)
        {
            if (_deviceService.IsSendingAllowed)
            {
                var payload = new
                {
                    DeviceState = "active",
                    TimeStamp = DateTime.Now
                };

                if (await _deviceService.SendDataAsync(JsonConvert.SerializeObject(payload)))

                    await Task.Delay(1000);
            }
            else
                await _deviceService.SendDataAsync(JsonConvert.SerializeObject("Sending not allowed."));
        }
    }
}
