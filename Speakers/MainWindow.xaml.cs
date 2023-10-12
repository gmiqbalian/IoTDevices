using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Speakers
{
    public partial class MainWindow : Window
    {
        private readonly INetworkService _networkService;
        private readonly IDeviceService _deviceService;
        private readonly DeviceConfiguration _deviceConfiguration;
        public MainWindow(INetworkService networkService, IDeviceService deviceService, DeviceConfiguration deviceConfiguration)
        {
            InitializeComponent();

            _networkService = networkService;
            _deviceService = deviceService;
            _deviceConfiguration = deviceConfiguration;

            Task.WhenAll(TestConnectivityAsync(),
                ToggleSpeakersState(),
                SendTelemetryDataAsync()
                );
        }

        private async Task ToggleSpeakersState()
        {
            Storyboard machine = (Storyboard)this.FindResource("SpeakerStoryboard");

            while (true)
            {
                if (_deviceConfiguration.IsSendingAllowed)
                    machine.Begin();
                else
                    machine.Stop();

                await Task.Delay(5000);
            }
        }
        private async Task TestConnectivityAsync()
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
                if (_deviceConfiguration.IsSendingAllowed)
                {
                    var payload = new
                    {
                        BatteryLevel = "70%",
                        SpeakerTemp = "2",
                        DeviceState = "active",
                        TimeStamp = DateTime.Now
                    };
                    var json = JsonConvert.SerializeObject(payload);

                    if (await _deviceService.SendDataAsync(json))
                        MessageToCloud.Text = $"Message sent to cloud : {json}";

                    await Task.Delay(1000);
                };
            }
        }
    }
}
