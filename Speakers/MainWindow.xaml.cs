using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Speakers
{
    public partial class MainWindow : Window
    {
        private readonly NetworkService _networkService;
        private readonly DeviceService _deviceService;
        public MainWindow(NetworkService networkService, DeviceService deviceService)
        {

            Task.Delay(5000);

            InitializeComponent();

            _networkService = networkService;
            _deviceService = deviceService;


            Task.WhenAll(ConfigureDevice(),
                GetConnectionStatusAsync(),
                ToggleSpeakersState(),
                SendTelemetryDataAsync());

        }
        private async Task ConfigureDevice()
        {
            var twinCollection = new TwinCollection();
            twinCollection["deviceType"] = "speakers";
            twinCollection["location"] = "lounge";

            if(_deviceService.IsConfigured)
            {
                await _deviceService.UpdateTwinAsync(twinCollection);
                await _deviceService.RegisterDirectMethodToCloud();
            }
        }

        private async Task ToggleSpeakersState()
        {
            Storyboard machine = (Storyboard)this.FindResource("SpeakerStoryboard");

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
                        BatteryLevel = "70%",
                        SpeakerTemp = "2",
                        DeviceState = "active",
                        TimeStamp = DateTime.Now
                    };

                    await _deviceManager.SendTelemetryDataAsync(JsonConvert.SerializeObject(payload), 10000);
                    CloudMessage.Text = $"Volume: {payload.Volume}\nBattery: {payload.Battery}\nTime: {payload.Time}";
                }
            }
        }
        private async Task ToggleDeviceState()
        {
            Storyboard device = (Storyboard)FindResource("SpeakerStoryboard");
            while (true)
            {
                var state = string.Empty;
                if (_deviceManager.IsSendingAllowed)
                {
                    device.Begin();
                    state = "ON";
                    DeviceState.Text = $"{state}";
                }
                else
                    await _deviceService.SendDataAsync(JsonConvert.SerializeObject("Sending not allowed."));
            }
        }
    }
}
